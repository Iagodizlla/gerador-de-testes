using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.WebApp.Extensions;
using Gerador_de_testes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32;

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

        if (repositorioMateria.SelecionarRegistros()
            .Any(x => x.Nome.Equals(cadastrarVM.Nome, StringComparison.OrdinalIgnoreCase)))
        {
            ModelState.AddModelError("Nome", "Já existe uma matéria registrada com este nome.");
        }

        if (!ModelState.IsValid)
        {
            foreach (var cd in contatosDisponiveis)
                cadastrarVM.DisciplinasDisponiveis?.Add(new SelectListItem(cd.Nome, cd.Id.ToString()));

            return View(cadastrarVM);
        }

        var disciplinaSelecionada = contatosDisponiveis.FirstOrDefault(d => d.Id == cadastrarVM.DisciplinaSelecionada);

        if (disciplinaSelecionada is null)
        {
            ModelState.AddModelError("DisciplinaSelecionada", "A disciplina selecionada não foi encontrada.");

            foreach (var cd in contatosDisponiveis)
                cadastrarVM.DisciplinasDisponiveis?.Add(new SelectListItem(cd.Nome, cd.Id.ToString()));

            return View(cadastrarVM);
        }

        var registro = new Materia(cadastrarVM.Nome!, cadastrarVM.Serie, disciplinaSelecionada);

        registro.RegistrarDisciplina(disciplinaSelecionada);

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioMateria.CadastrarRegistro(registro);
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
        var disciplinasDisponiveis = repositorioDisciplina.SelecionarRegistros();

        if (repositorioMateria.SelecionarRegistros()
            .Any(x => x.Nome.Equals(editarVM.Nome, StringComparison.OrdinalIgnoreCase) && x.Id != id))
        {
            ModelState.AddModelError("Nome", "Já existe uma matéria registrada com este nome.");
        }

        if (!ModelState.IsValid)
        {
            foreach (var d in disciplinasDisponiveis)
                editarVM.DisciplinasDisponiveis?.Add(new SelectListItem(d.Nome, d.Id.ToString()));

            return View(editarVM);
        }

        var disciplinaSelecionada = disciplinasDisponiveis.FirstOrDefault(d => d.Id == editarVM.DisciplinaSelecionada);

        var materiaExistente = repositorioMateria.SelecionarRegistroPorId(id);

        materiaExistente.Nome = editarVM.Nome!;
        materiaExistente.Serie = editarVM.Serie;
        materiaExistente.RegistrarDisciplina(disciplinaSelecionada);

        var transacao = contexto.Database.BeginTransaction();

        try
        {
            repositorioMateria.EditarRegistro(id, materiaExistente);
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
        var possuiQAssociados = contexto.Questoes.Any(q => q.Materia.Id == id);

        if (possuiQAssociados)
        {
            ModelState.AddModelError("ExclusaoInvalida", "Não é possível excluir uma materia que possui questoes associados.");
            return RedirectToAction(nameof(Index));
        }

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