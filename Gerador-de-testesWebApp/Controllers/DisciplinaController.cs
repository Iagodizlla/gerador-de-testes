using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.WebApp.Extensions;
using Gerador_de_testes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gerador_de_testes.WebApp.Controllers;

[Route("disciplinas")]
public class DisciplinaController : Controller
{
    private readonly GeradorDeTestesDbContext contexto;
    private readonly IRepositorioDisciplina repositorioDisciplina;

    public DisciplinaController(GeradorDeTestesDbContext contexto, IRepositorioDisciplina repositorioDisciplina)
    {
        this.contexto = contexto;
        this.repositorioDisciplina = repositorioDisciplina;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = repositorioDisciplina.SelecionarRegistros();

        var visualizarVM = new VisualizarDisciplinasViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var cadastrarVM = new CadastrarDisciplinaViewModel();

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarDisciplinaViewModel cadastrarVM)
    {
        var registros = repositorioDisciplina.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (item.Nome.Equals(cadastrarVM.Nome))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma disciplina registrada com este nome.");
                return View(cadastrarVM);
            }
        }

        var entidade = cadastrarVM.ParaEntidade();

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioDisciplina.CadastrarRegistro(entidade);

            contexto.SaveChanges();

            transacao.Commit();
        }
        catch (Exception)
        {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("editar/{id:guid}")]
    public ActionResult Editar(Guid id)
    {
        var registroSelecionado = repositorioDisciplina.SelecionarRegistroPorId(id);

        var editarVM = new EditarDisciplinaViewModel(
            id,
            registroSelecionado.Nome
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarDisciplinaViewModel editarVM)
    {
        var registros = repositorioDisciplina.SelecionarRegistros();

        foreach (var item in registros)
        {
            if (!item.Id.Equals(id) && item.Nome.Equals(editarVM.Nome))
            {
                ModelState.AddModelError("CadastroUnico", "Já existe uma disciplina registrada com este nome.");
                return View(editarVM);

            }
        }

        var entidadeEditada = editarVM.ParaEntidade();

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioDisciplina.EditarRegistro(id, entidadeEditada);

            contexto.SaveChanges();

            transacao.Commit();
        }
        catch (Exception)
        {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("excluir/{id:guid}")]
    public IActionResult Excluir(Guid id)
    {
        var registroSelecionado = repositorioDisciplina.SelecionarRegistroPorId(id);

        var excluirVM = new ExcluirDisciplinaViewModel(registroSelecionado.Id, registroSelecionado.Nome);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioDisciplina.ExcluirRegistro(id);

            contexto.SaveChanges();

            transacao.Commit();
        }
        catch (Exception)
        {
            transacao.Rollback();

            throw;
        }

        return RedirectToAction(nameof(Index));
    }
}