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

            repositorioTestes.CadastrarRegistro(teste);

            return RedirectToAction("Index");
        }

        [HttpPost("SortearQuestoes")]
        public IActionResult SortearQuestoes(CadastrarTesteViewModel model)
        {
            var questoes = repositorioQuestoes.SelecionarRegistros()
                .Where(q => q.Materia.Id == model.MateriaId && q.Materia.Serie == model.Serie)
                .OrderBy(q => Guid.NewGuid())
                .Take(model.QteQuestoes)
                .ToList();

            return PartialView("_QuestoesSorteadas", questoes);
        }


    }
}
