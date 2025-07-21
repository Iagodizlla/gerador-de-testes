using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testesWebApp.Extensions;
using Gerador_de_testesWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace Gerador_de_testesWebApp.Controllers
{
    [Route("testes")]
    public class TesteController : Controller
    {
        private readonly GeradorDeTestesDbContext contexto;
        private readonly IRepositorioTeste repositorioTestes;
        private readonly IRepositorioDisciplina repositorioDisciplinas;
        private readonly IRepositorioMateria repositorioMaterias;
        private readonly IRepositorioQuestao repositorioQuestoes;

        public TesteController(
            GeradorDeTestesDbContext contexto,
            IRepositorioTeste repositorioTestes,
            IRepositorioDisciplina repositorioDisciplinas,
            IRepositorioMateria repositorioMaterias,
            IRepositorioQuestao repositorioQuestoes)
        {
            this.contexto = contexto;
            this.repositorioTestes = repositorioTestes;
            this.repositorioDisciplinas = repositorioDisciplinas;
            this.repositorioMaterias = repositorioMaterias;
            this.repositorioQuestoes = repositorioQuestoes;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var registros = repositorioTestes.SelecionarRegistros();

            var visualizarVM = new VisualizarTesteViewModel(registros);

            return View(visualizarVM);
        }

        [HttpGet("cadastrar")]
        public IActionResult Cadastrar()
        {
            var disciplinas = repositorioDisciplinas.SelecionarRegistros();
            var materias = repositorioMaterias.SelecionarRegistros();

            var viewModel = new CadastrarTesteViewModel(disciplinas, materias);

            return View(viewModel);
        }


        [HttpPost("cadastrar")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(CadastrarTesteViewModel cadastrarVM)
        {
            if (!ModelState.IsValid)
            {
                var disciplinas = repositorioDisciplinas.SelecionarRegistros();
                var materias = repositorioMaterias.SelecionarRegistros();
                cadastrarVM.DisciplinasDisponiveis = disciplinas.Select(d => new SelectListItem(d.Nome, d.Id.ToString())).ToList();
                cadastrarVM.MateriasDisponiveis = materias.Select(m => new SelectListItem(m.Nome, m.Id.ToString())).ToList();
                return View(cadastrarVM);
            }

            var disciplina = repositorioDisciplinas.SelecionarRegistroPorId(cadastrarVM.DisciplinaId);
            var materia = repositorioMaterias.SelecionarRegistroPorId(cadastrarVM.MateriaId);

            var questoesFiltradas = repositorioQuestoes.SelecionarRegistros()
                .Where(q => q.Materia.Id == cadastrarVM.MateriaId && q.Materia.Serie == cadastrarVM.Serie)
                .ToList();

            var random = new Random();
            var questoesSorteadas = questoesFiltradas
                .OrderBy(q => random.Next())
                .Take(cadastrarVM.QteQuestoes)
                .ToList();

            var teste = new Teste(
                cadastrarVM.Titulo,
                new List<Disciplina> { disciplina },
                cadastrarVM.Serie,
                new List<Materia> { materia },
                cadastrarVM.QteQuestoes,
                questoesSorteadas
            );

            var transacao = contexto.Database.BeginTransaction();

            try
            {
                repositorioTestes.CadastrarRegistro(teste);
                contexto.SaveChanges();
                transacao.Commit();
            }
            catch
            {
                transacao.Rollback();
                throw;
            }

            return RedirectToAction("Index");
        }

        [HttpPost("SortearQuestoes")]
        [ValidateAntiForgeryToken]
        public IActionResult SortearQuestoes(CadastrarTesteViewModel model)
        {
            var questoes = repositorioQuestoes.SelecionarRegistros()
                .Where(q => q.Materia.Id == model.MateriaId && q.Materia.Serie == model.Serie)
                .OrderBy(q => Guid.NewGuid())
                .Take(model.QteQuestoes)
                .ToList();

            return PartialView("_QuestoesSorteadas", questoes);
        }

        [HttpGet("editar/{id}")]
        public IActionResult Editar(Guid id)
        {
            var teste = repositorioTestes.SelecionarRegistroPorId(id);
            if (teste == null)
            {
                return NotFound();
            }
            var disciplinas = repositorioDisciplinas.SelecionarRegistros();
            var materias = repositorioMaterias.SelecionarRegistros();
            var editarVM = new EditarTesteViewModel(teste, disciplinas, materias);
            return View(editarVM);
        }

        [HttpPost("editar/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Guid id, EditarTesteViewModel editarVM)
        {
            if (!ModelState.IsValid)
            {
                var disciplinas = repositorioDisciplinas.SelecionarRegistros();
                var materias = repositorioMaterias.SelecionarRegistros();
                editarVM.DisciplinasDisponiveis = disciplinas.Select(d => new SelectListItem(d.Nome, d.Id.ToString())).ToList();
                editarVM.MateriasDisponiveis = materias.Select(m => new SelectListItem(m.Nome, m.Id.ToString())).ToList();
                return View(editarVM);
            }
            var testeExistente = repositorioTestes.SelecionarRegistroPorId(id);
            if (testeExistente == null)
            {
                return NotFound();
            }

            var disciplina = repositorioDisciplinas.SelecionarRegistroPorId(editarVM.DisciplinaId);
            var materia = repositorioMaterias.SelecionarRegistroPorId(editarVM.MateriaId);
            var questoesFiltradas = repositorioQuestoes.SelecionarRegistros()
                .Where(q => q.Materia.Id == editarVM.MateriaId && q.Materia.Serie == editarVM.Serie)
                .ToList();
            var random = new Random();
            var questoesSorteadas = questoesFiltradas
                .OrderBy(q => random.Next())
                .Take(editarVM.QteQuestoes)
                .ToList();
            testeExistente.AtualizarRegistro(new Teste(
                editarVM.Titulo,
                new List<Disciplina> { disciplina },
                editarVM.Serie,
                new List<Materia> { materia },
                editarVM.QteQuestoes,
                questoesSorteadas
            ));

            var transacao = contexto.Database.BeginTransaction();

            try
            {
                repositorioTestes.EditarRegistro(id, testeExistente);
                contexto.SaveChanges();
                transacao.Commit();
            }
            catch
            {
                transacao.Rollback();
                throw;
            }

            return RedirectToAction("Index");
        }

        [HttpGet("excluir/{id:guid}")]
        public IActionResult Excluir(Guid id, ExcluirTesteViewModel excluirVM)
        {
            var registroSelecionado = repositorioTestes.SelecionarRegistroPorId(id);
            excluirVM = new ExcluirTesteViewModel(registroSelecionado.Titulo);

            if (registroSelecionado == null)
            {
                ModelState.AddModelError("RegistroNaoEncontrado", "Teste não encontrado.");
                return RedirectToAction(nameof(Index));
            }

            return View(excluirVM);
        }

        [HttpPost("excluir/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult Excluir(Guid id)
        {
            var registroSelecionado = repositorioTestes.SelecionarRegistroPorId(id);
            if (registroSelecionado == null)
            {
                ModelState.AddModelError("RegistroNaoEncontrado", "Teste não encontrado.");
                return RedirectToAction(nameof(Index));
            }

            var transacao = contexto.Database.BeginTransaction();

            try
            {
                repositorioTestes.ExcluirRegistro(id);
                contexto.SaveChanges();
                transacao.Commit();
            }
            catch
            {
                transacao.Rollback();
                throw;
            }

            return RedirectToAction("Index");
        }
    }
}
