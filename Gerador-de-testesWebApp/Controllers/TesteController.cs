using DinkToPdf;
using DinkToPdf.Contracts;
using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testesWebApp.Extensions;
using Gerador_de_testesWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Runtime.CompilerServices;
using static System.Net.Mime.MediaTypeNames;

namespace Gerador_de_testesWebApp.Controllers
{
    [Route("testes")]
    public class TesteController : Controller
    {
        private readonly IConverter _pdfConverter;
        private readonly ICompositeViewEngine _viewEngine;
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
            IRepositorioQuestao repositorioQuestoes,
            IConverter pdfConverter,
            ICompositeViewEngine viewEngine)
        {
            this.contexto = contexto;
            this.repositorioTestes = repositorioTestes;
            this.repositorioDisciplinas = repositorioDisciplinas;
            this.repositorioMaterias = repositorioMaterias;
            this.repositorioQuestoes = repositorioQuestoes;
            _pdfConverter = pdfConverter;
            _viewEngine = viewEngine;
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

        [HttpGet("realizar/{id}")]
        public IActionResult Realizar(Guid id)
        {
            var teste = repositorioTestes.SelecionarRegistroPorId(id);
            if (teste == null)
                return NotFound();

            // Crie um ViewModel que contenha o teste, questões e alternativas
            var realizarVM = new RealizarTesteViewModel(teste);
            return View(realizarVM);
        }

        [HttpPost("realizar/{id}")]
        public IActionResult Realizar(Guid id, RealizarTesteViewModel model)
        {
            var teste = repositorioTestes.SelecionarRegistroPorId(id);
            if (teste == null)
                return NotFound();

            // Verifica as respostas
            var resultado = new List<bool>();
            for (int i = 0; i < teste.QuestoesSelecionadas.Count; i++)
            {
                var questao = teste.QuestoesSelecionadas[i];
                var respostaUsuario = model.Respostas[i];
                var correta = questao.Alternativas.Any(a => a.Id == respostaUsuario && a.Correta);
                resultado.Add(correta);
            }

            model.Resultados = resultado;
            model.Titulo = teste.Titulo;
            model.Questoes = teste.QuestoesSelecionadas;

            return View("ResultadoTeste", model);
        }

        [HttpGet("gerar-pdf/{id}")]
        public IActionResult GerarPdf(Guid id)
        {
            var teste = repositorioTestes.SelecionarRegistroPorId(id);

            var testePdf = RenderRazorViewToString(this, "PdfTeste", teste);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
                DocumentTitle = teste.Titulo
                },
                Objects = {
                    new ObjectSettings() {
                    HtmlContent = testePdf,
                    }
                }
            };

            var pdf = _pdfConverter.Convert(doc);
            return File(pdf, "application/pdf", $"{teste.Titulo}.pdf");
        }

        private string RenderRazorViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using var sw = new StringWriter();
            var viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, false);
            var viewContext = new ViewContext(
                controller.ControllerContext,
                viewResult.View,
                controller.ViewData,
                controller.TempData,
                sw,
                new HtmlHelperOptions()
            );


            viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();

            return sw.ToString();
        }

        [HttpGet("gabarito-pdf/{id}")]
        public IActionResult GerarGabaritoPdf(Guid id)
        {
            var teste = repositorioTestes.SelecionarRegistroPorId(id);
            if (teste == null)
                return NotFound();

            var html = RenderRazorViewToString(this, "GabaritoPdf", teste);

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                PaperSize = PaperKind.A4,
                Orientation = Orientation.Portrait,
                DocumentTitle = "Gabarito - " + teste.Titulo
                },
               
                Objects = {
                    new ObjectSettings() {
                    HtmlContent = html
                    }
                }
            };

            var pdf = _pdfConverter.Convert(doc);
            return File(pdf, "application/pdf", $"Gabarito_{teste.Titulo}.pdf");
        }

    }
}