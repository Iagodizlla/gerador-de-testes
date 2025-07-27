using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.WebApp.Models;

namespace TesteFacil.WebApp.Controllers;

[Route("questoes")]
public class QuestaoController : Controller
{
    private readonly IRepositorioQuestao repositorioQuestao;
    private readonly IRepositorioMateria repositorioMateria;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<QuestaoController> logger;

    public QuestaoController(
        IRepositorioQuestao repositorioQuestao,
        IRepositorioMateria repositorioMateria,
        IUnitOfWork unitOfWork,
        ILogger<QuestaoController> logger
    )
    {
        this.repositorioQuestao = repositorioQuestao;
        this.repositorioMateria = repositorioMateria;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = repositorioQuestao.SelecionarRegistros();

        var visualizarVM = new VisualizarQuestoesViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var materias = repositorioMateria.SelecionarRegistros();

        var cadastrarVM = new CadastrarQuestaoViewModel(materias);

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarQuestaoViewModel cadastrarVM)
    {
        var registros = repositorioQuestao.SelecionarRegistros();

        var materias = repositorioMateria.SelecionarRegistros();

        // Validação: enunciados duplicados
        if (registros.Any(i => i.Enunciado.Equals(cadastrarVM.Enunciado)))
        {
            ModelState.AddModelError(
                "CadastroUnico",
                "Já existe uma questão registrada com este enunciado."
            );

            cadastrarVM.MateriasDisponiveis = materias
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList();

            return View(cadastrarVM);
        }

        try
        {
            var entidade = CadastrarQuestaoViewModel.ParaEntidade(cadastrarVM, materias);

            repositorioQuestao.Cadastrar(entidade);

            unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@ViewModel}.",
                cadastrarVM
            );
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("cadastrar/adicionar-alternativa")]
    public IActionResult AdicionarAlternativa(
        CadastrarQuestaoViewModel cadastrarVm,
        AdicionarAlternativaQuestaoViewModel alternativaVm
    )
    {
        cadastrarVm.MateriasDisponiveis = repositorioMateria
            .SelecionarRegistros()
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

        cadastrarVm.MateriasDisponiveis = repositorioMateria
            .SelecionarRegistros()
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();

        return View(nameof(Cadastrar), cadastrarVm);
    }

    [HttpGet("editar/{id:guid}")]
    public IActionResult Editar(Guid id)
    {
        var registro = repositorioQuestao.SelecionarRegistroPorId(id);

        if (registro is null)
            return RedirectToAction(nameof(Index));

        var materias = repositorioMateria.SelecionarRegistros();

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
        var registros = repositorioQuestao.SelecionarRegistros();

        var materias = repositorioMateria.SelecionarRegistros();

        if (registros.Any(i => !i.Id.Equals(id) && i.Enunciado.Equals(editarVm.Enunciado)))
        {
            ModelState.AddModelError(
                "CadastroUnico",
                "Já existe uma questão registrada com este enunciado."
            );

            editarVm.MateriasDisponiveis = materias
                .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
                .ToList();

            return View(editarVm);
        }

        try
        {
            var entidadeEditada = EditarQuestaoViewModel.ParaEntidade(editarVm, materias);

            repositorioQuestao.Editar(id, entidadeEditada);

            unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante o registro de {@ViewModel}.",
                editarVm
            );
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost("editar/{id:guid}/adicionar-alternativa")]
    public IActionResult AdicionarAlternativa(
       EditarQuestaoViewModel editarVm,
       AdicionarAlternativaQuestaoViewModel alternativaVm
   )
    {
        editarVm.MateriasDisponiveis = repositorioMateria
            .SelecionarRegistros()
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

        var registro = repositorioQuestao.SelecionarRegistroPorId(editarVm.Id);

        if (registro is null)
            return RedirectToAction(nameof(Index));

        registro.AdicionarAlternativa(alternativaVm.Resposta, alternativaVm.Correta);

        unitOfWork.Commit();

        return View(nameof(Editar), editarVm);
    }

    [HttpPost("editar/{id:guid}/remover-alternativa/{letra:alpha}")]
    public IActionResult RemoverAlternativa(char letra, EditarQuestaoViewModel editarVm)
    {
        editarVm.MateriasDisponiveis = repositorioMateria
            .SelecionarRegistros()
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();

        var alternativa = editarVm.AlternativasSelecionadas
            .Find(a => a.Letra.Equals(letra));

        if (alternativa is not null)
        {
            var registro = repositorioQuestao.SelecionarRegistroPorId(editarVm.Id);

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
        var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

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
        try
        {
            repositorioQuestao.Excluir(id);

            unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            unitOfWork.Rollback();

            logger.LogError(
                ex,
                "Ocorreu um erro durante a exclusão do registro {Id}.",
                id
            );
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("detalhes/{id:guid}")]
    public IActionResult Detalhes(Guid id)
    {
        var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var detalhesVM = new DetalhesQuestaoViewModel(
            id,
            registroSelecionado.Enunciado,
            registroSelecionado.Materia.Nome,
            registroSelecionado.UtilizadaEmTeste ? "Sim" : "Não",
            registroSelecionado.AlternativaCorreta?.Resposta ?? string.Empty,
            registroSelecionado.Alternativas
        );

        return View(detalhesVM);
    }
}