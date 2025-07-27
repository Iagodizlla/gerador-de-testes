using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testes.WebApp.Models;
using Gerador_de_testesWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;
using TesteFacil.WebApp.Models;

namespace TesteFacil.WebApp.Controllers;

[Route("testes")]
public class TesteController : Controller
{
    private readonly IRepositorioTeste repositorioTeste;
    private readonly IRepositorioQuestao repositorioQuestao;
    private readonly IRepositorioDisciplina repositorioDisciplina;
    private readonly IRepositorioMateria repositorioMateria;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<TesteController> logger;

    public TesteController(
        IRepositorioTeste repositorioTeste,
        IRepositorioQuestao repositorioQuestao,
        IRepositorioDisciplina repositorioDisciplina,
        IRepositorioMateria repositorioMateria,
        IUnitOfWork unitOfWork,
        ILogger<TesteController> logger
    )
    {
        this.repositorioTeste = repositorioTeste;
        this.repositorioQuestao = repositorioQuestao;
        this.repositorioDisciplina = repositorioDisciplina;
        this.repositorioMateria = repositorioMateria;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = repositorioTeste.SelecionarRegistros();

        var visualizarVm = new VisualizarTestesViewModel(registros);

        return View(visualizarVm);
    }

    [HttpGet("gerar/primeira-etapa")]
    public IActionResult PrimeiraEtapaGerar()
    {
        var disciplinas = repositorioDisciplina.SelecionarRegistros();

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
        var registros = repositorioTeste.SelecionarRegistros();

        var disciplinas = repositorioDisciplina.SelecionarRegistros();

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

        var disciplinaSelecionada = repositorioDisciplina.SelecionarRegistroPorId(primeiraEtapaVm.DisciplinaId);

        if (disciplinaSelecionada is null)
            return RedirectToAction(nameof(Index));

        var materias = repositorioMateria
            .SelecionarRegistros()
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
        var disciplinas = repositorioDisciplina.SelecionarRegistros();
        var materias = repositorioMateria.SelecionarRegistros();
        var questoes = repositorioQuestao.SelecionarRegistros();

        var entidade = SegundaEtapaGerarTesteViewModel.ParaEntidade(
            segundaEtapaVm,
            disciplinas,
            materias,
            questoes
        );

        var questoesSorteadas = entidade.SortearQuestoes();

        if (questoesSorteadas is null)
            return RedirectToAction(nameof(Index));

        segundaEtapaVm.QuestoesSorteadas = questoesSorteadas.Select(q => new DetalhesQuestaoViewModel(
            q.Id,
            q.Enunciado,
            q.Materia.Nome,
            q.UtilizadaEmTeste ? "Sim" : "Não",
            q.AlternativaCorreta?.Resposta ?? string.Empty,
            q.Alternativas
        )).ToList();

        segundaEtapaVm.MateriasDisponiveis = repositorioMateria
            .SelecionarRegistros()
            .Where(m => m.Disciplina.Id.Equals(segundaEtapaVm.DisciplinaId))
            .Where(m => m.Serie.Equals(segundaEtapaVm.Serie))
            .Select(m => new SelectListItem(m.Nome, m.Id.ToString()))
            .ToList();

        return View(nameof(SegundaEtapaGerar), segundaEtapaVm);
    }

    [HttpPost("gerar/confirmar")]
    public IActionResult ConfirmarGeracao(SegundaEtapaGerarTesteViewModel segundaEtapaVm)
    {
        var disciplinas = repositorioDisciplina.SelecionarRegistros();
        var materias = repositorioMateria.SelecionarRegistros();
        var questoes = repositorioQuestao.SelecionarRegistros();

        try
        {
            var entidade = SegundaEtapaGerarTesteViewModel.ParaEntidade(
                segundaEtapaVm,
                disciplinas,
                materias,
                questoes
            );

            repositorioTeste.Cadastrar(entidade);

            unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@ViewModel}.",
                segundaEtapaVm
            );
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        var registroSelecionado = repositorioTeste.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var detalhesVM = DetalhesTesteViewModel.ParaDetalhesVm(registroSelecionado);

        return View(detalhesVM);
    }
}