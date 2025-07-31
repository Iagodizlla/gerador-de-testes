using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using TesteFacil.Aplicacao.ModuloDisciplina;
using TesteFacil.Aplicacao.ModuloMateria;
using TesteFacil.Aplicacao.ModuloQuestao;
using TesteFacil.Aplicacao.ModuloTeste;
using TesteFacil.Dominio.Extensions;
using TesteFacil.WebApp.Models;

namespace TesteFacil.WebApp.Controllers;

[Route("testes")]
public class TesteController : Controller
{
    private readonly TesteAppService testeAppService;
    private readonly QuestaoAppService questaoAppService;
    private readonly MateriaAppService materiaAppService;
    private readonly DisciplinaAppService disciplinaAppService;

    public TesteController(
        TesteAppService testeAppService,
        QuestaoAppService questaoAppService,
        MateriaAppService materiaAppService,
        DisciplinaAppService disciplinaAppService
    )
    {
        this.testeAppService = testeAppService;
        this.questaoAppService = questaoAppService;
        this.materiaAppService = materiaAppService;
        this.disciplinaAppService = disciplinaAppService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var resultado = testeAppService.SelecionarTodos();

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                var notificacaoJson = NotificacaoViewModel.GerarNotificacaoSerializada(
                    erro.Message,
                    erro.Reasons[0].Message
                );

                TempData.Add(nameof(NotificacaoViewModel), notificacaoJson);
                break;
            }

            return RedirectToAction("erro", "home");
        }

        var visualizarVm = new VisualizarTestesViewModel(resultado.ValueOrDefault);

        var existeNotificacao = TempData.TryGetValue(nameof(NotificacaoViewModel), out var valor);

        if (existeNotificacao && valor is string jsonString)
        {
            var notificacaoVm = JsonSerializer.Deserialize<NotificacaoViewModel>(jsonString);

            ViewData.Add(nameof(NotificacaoViewModel), notificacaoVm);
        }

        return View(visualizarVm);
    }

    [HttpGet("gerar/primeira-etapa")]
    public IActionResult PrimeiraEtapaGerar()
    {
        var disciplinas = disciplinaAppService.SelecionarTodos().ValueOrDefault;

        var primeiraEtapaVm = new PrimeiraEtapaGerarTesteViewModel
        {
            DisciplinasDisponiveis = disciplinas
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList()
        };

        return View(primeiraEtapaVm);
    }

    [HttpPost("gerar/primeira-etapa")]
    public IActionResult PrimeiraEtapaGerar(PrimeiraEtapaGerarTesteViewModel primeiraEtapaVm)
    {
        var registros = testeAppService.SelecionarTodos().ValueOrDefault;

        var disciplinas = disciplinaAppService.SelecionarTodos().ValueOrDefault;

        if (registros.Any(i => i.Titulo.Equals(primeiraEtapaVm.Titulo)))
        {
            ModelState.AddModelError(
                "CadastroUnico",
                "Já existe um teste registrado com este nome."
            );

            primeiraEtapaVm.DisciplinasDisponiveis = disciplinas
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList();

            return View(primeiraEtapaVm);
        }

        var disciplinaSelecionada = disciplinaAppService.SelecionarPorId(primeiraEtapaVm.DisciplinaId).Value;

        if (disciplinaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var materias = materiaAppService
            .SelecionarTodos()
            .ValueOrDefault
            .Where(m => m.Disciplina.Equals(disciplinaSelecionada))
            .Where(m => m.Serie.Equals(primeiraEtapaVm.Serie))
            .ToList();

        var segundaEtapaVm = PrimeiraEtapaGerarTesteViewModel.AvancarEtapa(
            primeiraEtapaVm,
            disciplinaSelecionada,
            materias
        );

        var jsonString = JsonSerializer.Serialize(segundaEtapaVm);

        TempData.Add(nameof(SegundaEtapaGerarTesteViewModel), jsonString);

        return RedirectToAction(nameof(SegundaEtapaGerar));
    }

    [HttpGet("gerar/segunda-etapa")]
    public IActionResult SegundaEtapaGerar()
    {
        var conseguiuRecuperar = TempData
            .TryGetValue(nameof(SegundaEtapaGerarTesteViewModel), out var value);

        if (!conseguiuRecuperar || value is not string jsonString)
            return RedirectToAction(nameof(Index));

        var segundaEtapaVm = JsonSerializer.Deserialize<SegundaEtapaGerarTesteViewModel>(jsonString);

        return View(segundaEtapaVm);
    }

    [HttpPost("gerar/segunda-etapa/sortear-questoes")]
    public IActionResult SortearQuestoes(SegundaEtapaGerarTesteViewModel segundaEtapaVm)
    {
        var disciplinas = disciplinaAppService.SelecionarTodos().ValueOrDefault;
        var materias = materiaAppService.SelecionarTodos().ValueOrDefault;
        var questoes = questaoAppService.SelecionarTodos().ValueOrDefault;

        var entidade = SegundaEtapaGerarTesteViewModel.ParaEntidade(
            segundaEtapaVm,
            disciplinas,
            materias,
            questoes
        );

        var questoesSorteadas = entidade.SortearQuestoes();

        if (questoesSorteadas is null)
            return RedirectToAction(nameof(Index));

        segundaEtapaVm.QuestoesSorteadas = questoesSorteadas
            .Select(DetalhesQuestaoViewModel.ParaDetalhesVm)
            .ToList();

        segundaEtapaVm.MateriasDisponiveis = materias
            .Where(m => m.Disciplina.Id.Equals(segundaEtapaVm.DisciplinaId))
            .Where(m => m.Serie.Equals(segundaEtapaVm.Serie))
            .Select(m => new SelectListItem(m.Nome, m.Id.ToString()))
            .ToList();

        return View(nameof(SegundaEtapaGerar), segundaEtapaVm);
    }

    [HttpPost("gerar/confirmar")]
    public IActionResult ConfirmarGeracao(SegundaEtapaGerarTesteViewModel segundaEtapaVm)
    {
        var disciplinas = disciplinaAppService.SelecionarTodos().ValueOrDefault;
        var materias = materiaAppService.SelecionarTodos().ValueOrDefault;
        var questoes = questaoAppService.SelecionarTodos().ValueOrDefault;

        var entidade = SegundaEtapaGerarTesteViewModel.ParaEntidade(
            segundaEtapaVm,
            disciplinas,
            materias,
            questoes
        );

        var resultado = testeAppService.Cadastrar(entidade);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                var notificacaoJson = NotificacaoViewModel.GerarNotificacaoSerializada(
                    erro.Message,
                    erro.Reasons[0].Message
                );

                TempData.Add(nameof(NotificacaoViewModel), notificacaoJson);
                break;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        var resultado = testeAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                var notificacaoJson = NotificacaoViewModel.GerarNotificacaoSerializada(
                    erro.Message,
                    erro.Reasons[0].Message
                );

                TempData.Add(nameof(NotificacaoViewModel), notificacaoJson);
                break;
            }

            return RedirectToAction(nameof(Index));
        }

        var registro = resultado.Value;

        var excluirVM = new ExcluirTesteViewModel(
            registro.Id,
            registro.Titulo
        );

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        var resultado = testeAppService.Excluir(id);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                var notificacaoJson = NotificacaoViewModel.GerarNotificacaoSerializada(
                    erro.Message,
                    erro.Reasons[0].Message
                );

                TempData.Add(nameof(NotificacaoViewModel), notificacaoJson);
                break;
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        var resultado = testeAppService.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                var notificacaoJson = NotificacaoViewModel.GerarNotificacaoSerializada(
                    erro.Message,
                    erro.Reasons[0].Message
                );

                TempData.Add(nameof(NotificacaoViewModel), notificacaoJson);
                break;
            }

            return RedirectToAction(nameof(Index));
        }

        var detalhesVm = DetalhesTesteViewModel.ParaDetalhesVm(resultado.Value);

        return View(detalhesVm);
    }
}