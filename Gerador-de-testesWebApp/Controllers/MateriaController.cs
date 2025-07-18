using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.WebApp.Extensions;
using Gerador_de_testes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gerador_de_testes.WebApp.Controllers;

[Route("materias")]
public class MateriaController : Controller
{
    private readonly GeradorDeTestesDbContext contexto;
    private readonly IRepositorioMateria repositorioMateria;
    private readonly IRepositorioDisciplina repositorioDisciplina;

    public MateriaController(
        GeradorDeTestesDbContext contexto,
        IRepositorioMateria repositorioCompromisso,
        IRepositorioDisciplina repositorioContato

    )
    {
        this.contexto = contexto;
        this.repositorioMateria = repositorioCompromisso;
        this.repositorioDisciplina = repositorioContato;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var registros = repositorioMateria.SelecionarRegistros();

        var visualizarVM = new VisualizarMateriasViewModel(registros);

        return View(visualizarVM);
    }

    [HttpGet("cadastrar")]
    public IActionResult Cadastrar()
    {
        var contatosDisponiveis = repositorioDisciplina.SelecionarRegistros();

        var cadastrarVM = new CadastrarMateriaViewModel(contatosDisponiveis);

        return View(cadastrarVM);
    }

    [HttpPost("cadastrar")]
    [ValidateAntiForgeryToken]
    public IActionResult Cadastrar(CadastrarMateriaViewModel cadastrarVM)
    {
        var contatosDisponiveis = repositorioDisciplina.SelecionarRegistros();

        if (!ModelState.IsValid)
        {
            foreach (var cd in contatosDisponiveis)
            {
                var selecionarVM = new SelectListItem(cd.Nome, cd.Id.ToString());

                cadastrarVM.DisciplinasDisponiveis?.Add(selecionarVM);
            }

            return View(cadastrarVM);
        }

        var despesa = cadastrarVM.ParaEntidade(contatosDisponiveis);

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioMateria.CadastrarRegistro(despesa);

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
        var disciplinasDisponiveis = repositorioDisciplina.SelecionarRegistros();

        var registroSelecionado = repositorioMateria.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var editarVM = new EditarMateriaViewModel(
            id,
            registroSelecionado.Nome,
            registroSelecionado.Serie,
            registroSelecionado.Disciplina,
            disciplinasDisponiveis
        );

        return View(editarVM);
    }

    [HttpPost("editar/{id:guid}")]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(Guid id, EditarMateriaViewModel editarVM)
    {
        var contatosDisponiveis = repositorioDisciplina.SelecionarRegistros();

        if (!ModelState.IsValid)
        {
            foreach (var cd in contatosDisponiveis)
            {
                var selecionarVM = new SelectListItem(cd.Nome, cd.Id.ToString());

                editarVM.DisciplinasDisponiveis?.Add(selecionarVM);
            }

            return View(editarVM);
        }

        var compromissoEditado = editarVM.ParaEntidade(contatosDisponiveis);

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioMateria.EditarRegistro(id, compromissoEditado);

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
        var registroSelecionado = repositorioMateria.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var excluirVM = new ExcluirMateriaViewModel(registroSelecionado.Id, registroSelecionado.Nome);

        return View(excluirVM);
    }

    [HttpPost("excluir/{id:guid}")]
    [ValidateAntiForgeryToken]
    public IActionResult ExcluirConfirmado(Guid id)
    {
        var registroSelecionado = repositorioMateria.SelecionarRegistroPorId(id);

        if (registroSelecionado is null)
            return RedirectToAction(nameof(Index));

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioMateria.ExcluirRegistro(id);

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