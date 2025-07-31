using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using TesteFacil.Aplicacao.ModuloMateria;
using TesteFacil.Aplicacao.ModuloQuestao;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.WebApp.Models;

namespace TesteFacil.WebApp.Controllers;

[Route("questoes")]
public class QuestaoController : Controller
{
    private readonly QuestaoAppService questaoAppService;
    private readonly MateriaAppService materiaAppService;

    public QuestaoController(QuestaoAppService questaoAppService, MateriaAppService materiaAppService)
    {
        this.questaoAppService = questaoAppService;
        this.materiaAppService = materiaAppService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var resultado = questaoAppService.SelecionarTodos();

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

        var visualizarVM = new VisualizarQuestoesViewModel(resultado.ValueOrDefault);

        var existeNotificacao = TempData.TryGetValue(nameof(NotificacaoViewModel), out var valor);

        if (existeNotificacao && valor is string jsonString)
        {
            var notificacaoVm = JsonSerializer.Deserialize<NotificacaoViewModel>(jsonString);

            ViewData.Add(nameof(NotificacaoViewModel), notificacaoVm);
        }

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var materias = materiaAppService.SelecionarTodos().ValueOrDefault;

        var cadastrarVM = new CadastrarQuestaoViewModel(materias);

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarQuestaoViewModel cadastrarVM)
    {
        var materias = materiaAppService.SelecionarTodos().ValueOrDefault;

        var entidade = CadastrarQuestaoViewModel.ParaEntidade(cadastrarVM, materias);

        var resultado = questaoAppService.Cadastrar(entidade);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                if (erro.Metadata["TipoErro"].ToString() == "RegistroDuplicado")
                {
                    ModelState.AddModelError("CadastroUnico", erro.Reasons[0].Message);
                    break;
                }
            }

            cadastrarVM.MateriasDisponiveis = materias
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList();

            return View(cadastrarVM);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("cadastrar/adicionar-alternativa")]
    public IActionResult AdicionarAlternativa(
        CadastrarQuestaoViewModel cadastrarVm,
        AdicionarAlternativaQuestaoViewModel alternativaVm
    )
    {
        cadastrarVm.MateriasDisponiveis = materiaAppService
            .SelecionarTodos()
            .ValueOrDefault
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();

        if (cadastrarVm.AlternativasSelecionadas.Any(a => a.Resposta.Equals(alternativaVm.Resposta)))
        {
            ModelState.AddModelError(
                "CadastroUnico",
                "Já existe uma alternativa registrada com esta resposta."
            );

            return View(nameof(Cadastrar), cadastrarVm);
        }

        if (alternativaVm.Correta && cadastrarVm.AlternativasSelecionadas.Any(a => a.Correta))
        {
            ModelState.AddModelError(
                "CadastroUnico",
                "Já existe uma alternativa registrada como correta."
            );

            return View(nameof(Cadastrar), cadastrarVm);
        }

        cadastrarVm.AdicionarAlternativa(alternativaVm);

        return View(nameof(Cadastrar), cadastrarVm);
    }

    [HttpPost("cadastrar/remover-alternativa/{letra:alpha}")]
    public IActionResult RemoverAlternativa(char letra, CadastrarQuestaoViewModel cadastrarVm)
    {
        var alternativa = cadastrarVm.AlternativasSelecionadas
            .Find(a => a.Letra.Equals(letra));

        if (alternativa is not null)
            cadastrarVm.RemoverAlternativa(alternativa);

        cadastrarVm.MateriasDisponiveis = materiaAppService
            .SelecionarTodos()
            .ValueOrDefault
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();

        return View(nameof(Cadastrar), cadastrarVm);
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        var resultado = questaoAppService.SelecionarPorId(id);

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

        var materias = materiaAppService.SelecionarTodos().ValueOrDefault;

        var registro = resultado.Value;

        var editarVm = new EditarQuestaoViewModel(
            registro.Id,
            registro.Enunciado,
            registro.Materia.Id,
            registro.Alternativas,
            materias
        );

        return View(editarVm);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarQuestaoViewModel editarVm)
    {
        var materias = materiaAppService.SelecionarTodos().ValueOrDefault;

        var entidadeEditada = EditarQuestaoViewModel.ParaEntidade(editarVm, materias);

        var resultado = questaoAppService.Editar(id, entidadeEditada);

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                if (erro.Metadata["TipoErro"].ToString() == "RegistroDuplicado")
                {
                    ModelState.AddModelError("CadastroUnico", erro.Reasons[0].Message);
                    break;
                }
            }

            editarVm.MateriasDisponiveis = materias
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList();

            return View(editarVm);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("editar/{id:guid}/adicionar-alternativa")]
    public IActionResult AdicionarAlternativa(EditarQuestaoViewModel editarVm, AdicionarAlternativaQuestaoViewModel alternativaVm)
    {
        var resultado = questaoAppService.AdicionarAlternativaEmQuestao(
            editarVm.Id,
            alternativaVm.Resposta,
            alternativaVm.Correta
        );

        var materias = materiaAppService
            .SelecionarTodos()
            .ValueOrDefault
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();

        editarVm.MateriasDisponiveis = materias;

        if (resultado.IsFailed)
        {
            foreach (var erro in resultado.Errors)
            {
                if (erro.Metadata["TipoErro"].ToString() == "RegistroDuplicado")
                {
                    ModelState.AddModelError("CadastroUnico", erro.Reasons[0].Message);
                    break;
                }
            }

            return View(editarVm);
        }

        editarVm.AdicionarAlternativa(alternativaVm);

        return View(nameof(Editar), editarVm);
    }

    [HttpPost("editar/{id:guid}/remover-alternativa/{letra:alpha}")]
    public IActionResult RemoverAlternativa(char letra, EditarQuestaoViewModel editarVm)
    {
        var resultado = questaoAppService.RemoverAlternativaDeQuestao(letra, editarVm.Id);

        editarVm.MateriasDisponiveis = materiaAppService
            .SelecionarTodos()
            .ValueOrDefault
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();

        if (resultado.IsFailed)
            return View(nameof(Editar), editarVm);

        var alternativa = editarVm.AlternativasSelecionadas
            .Find(a => a.Letra.Equals(letra));

        if (alternativa is not null)
            editarVm.RemoverAlternativa(alternativa);

        return View(nameof(Editar), editarVm);
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        var resultado = questaoAppService.SelecionarPorId(id);

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

        var excluirVM = new ExcluirQuestaoViewModel(
            registro.Id,
            registro.Enunciado
        );

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        var resultado = questaoAppService.Excluir(id);

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
        var resultado = questaoAppService.SelecionarPorId(id);

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

        var detalhesVm = DetalhesQuestaoViewModel.ParaDetalhesVm(resultado.Value);

        return View(detalhesVm);
    }
}