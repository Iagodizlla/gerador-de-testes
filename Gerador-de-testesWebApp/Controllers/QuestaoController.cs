using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;
using TesteFacil.Aplicacao.ModuloDisciplina;
using TesteFacil.Aplicacao.ModuloMateria;
using TesteFacil.Aplicacao.ModuloQuestao;
using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.WebApp.Models;

namespace TesteFacil.WebApp.Controllers;

[Route("questoes")]
public class QuestaoController : Controller
{
    private readonly QuestaoAppService questaoAppService;
    private readonly MateriaAppService materiaAppService;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<QuestaoController> logger;

    public QuestaoController(
        QuestaoAppService repositorioQuestao,
        MateriaAppService repositorioMateria,
        IUnitOfWork unitOfWork,
        ILogger<QuestaoController> logger
    )
    {
        this.questaoAppService = repositorioQuestao;
        this.materiaAppService = repositorioMateria;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var resultado = questaoAppService.SelecionarRegistros();

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

        var registros = resultado.Value;

        var visualizarVM = new VisualizarQuestoesViewModel(registros);

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
        var materias = materiaAppService.SelecionarTodos();

        var valor = materias.Value;

        var cadastrarVM = new CadastrarQuestaoViewModel(valor);

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarQuestaoViewModel cadastrarVM)
    {
        var Resultadoregistros = questaoAppService.SelecionarRegistros();

        var registros = Resultadoregistros.Value;

        var Resultadomaterias = materiaAppService.SelecionarTodos();

        var materias = Resultadomaterias.Value;

        var entidade = FormularioQuestaoViewModel.ParaEntidade(cadastrarVM, materias);

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
        var Resultadomaterias = materiaAppService.SelecionarTodos();

        var materias = Resultadomaterias.Value;

        cadastrarVm.MateriasDisponiveis = materias
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList();

        // Validação: respostas duplicadas
        if (cadastrarVm.AlternativasSelecionadas.Any(a => a.Resposta.Equals(alternativaVm.Resposta)))
        {
            ModelState.AddModelError(
                "CadastroUnico",
                "Já existe uma alternativa registrada com esta resposta."
            );

            return View(nameof(Cadastrar), cadastrarVm);
        }

        // Validação: apenas uma alternativa correta
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

        var Resultadomaterias = materiaAppService.SelecionarTodos();

        var materias = Resultadomaterias.Value;

        cadastrarVm.MateriasDisponiveis = materias
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList();

        return View(nameof(Cadastrar), cadastrarVm);
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        var resultadoMaterias = materiaAppService.SelecionarTodos();

        var materias = resultadoMaterias.Value;

        var resultadoQuestao = questaoAppService.SelecionarRegistroPorId(id);

        if (resultadoQuestao.IsFailed)
        {
            foreach (var erro in resultadoQuestao.Errors)
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

        var registroSelecionado = resultadoQuestao.Value;

        var editarVM = new EditarQuestaoViewModel(
            id,
            registroSelecionado.Enunciado,
            registroSelecionado.Materia.Id,
            registroSelecionado.Alternativas,
            materias
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(Guid id, EditarQuestaoViewModel editarVm)
    {
        var resultadoMaterias = materiaAppService.SelecionarTodos();

        var materias = resultadoMaterias.Value;

        var entidadeEditada = FormularioQuestaoViewModel.ParaEntidade(editarVm, materias);

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
    public IActionResult AdicionarAlternativa(
       EditarQuestaoViewModel editarVm,
       AdicionarAlternativaQuestaoViewModel alternativaVm
   )
    {
        var Resultadomaterias = materiaAppService.SelecionarTodos();

        var materias = Resultadomaterias.Value;

        editarVm.MateriasDisponiveis = materias
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList();

        if (editarVm.AlternativasSelecionadas.Any(a => a.Resposta.Equals(alternativaVm.Resposta)))
        {
            ModelState.AddModelError(
                "CadastroUnico",
                "Já existe uma alternativa registrada com esta resposta."
            );

            return View(nameof(Editar), editarVm);
        }

        if (alternativaVm.Correta && editarVm.AlternativasSelecionadas.Any(a => a.Correta))
        {
            ModelState.AddModelError(
                "CadastroUnico",
                "Já existe uma alternativa registrada como correta."
            );

            return View(nameof(Editar), editarVm);
        }

        editarVm.AdicionarAlternativa(alternativaVm);

        var Resultadoregistro = questaoAppService.SelecionarRegistroPorId(editarVm.Id);
        var registro = Resultadoregistro.Value;

        if (registro is null)
            return RedirectToAction(nameof(Index));

        registro.AdicionarAlternativa(alternativaVm.Resposta, alternativaVm.Correta);

        unitOfWork.Commit();

        return View(nameof(Editar), editarVm);
    }

    [HttpPost("editar/{id:guid}/remover-alternativa/{letra:alpha}")]
    public IActionResult RemoverAlternativa(char letra, EditarQuestaoViewModel editarVm)
    {
        var Resultadomaterias = materiaAppService.SelecionarTodos();

        var materias = Resultadomaterias.Value;

        editarVm.MateriasDisponiveis = materias
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList();

        var alternativa = editarVm.AlternativasSelecionadas
            .Find(a => a.Letra.Equals(letra));

        if (alternativa is not null)
        {
            var Resultadoregistro = questaoAppService.SelecionarRegistroPorId(editarVm.Id);
            var registro = Resultadoregistro.Value;

            if (registro is null)
                return RedirectToAction(nameof(Index));

            editarVm.RemoverAlternativa(alternativa);

            registro.RemoverAlternativa(letra);

            unitOfWork.Commit();
        }

        return View(nameof(Editar), editarVm);
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        var resultado = questaoAppService.SelecionarRegistroPorId(id);

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

        var registroSelecionado = resultado.Value;

        var excluirVM = new ExcluirQuestaoViewModel(
            registroSelecionado.Id,
            registroSelecionado.Enunciado
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
        var resultado = questaoAppService.SelecionarRegistroPorId(id);

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