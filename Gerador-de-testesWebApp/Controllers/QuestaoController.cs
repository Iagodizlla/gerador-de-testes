using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testes.WebApp.Extensions;
using Gerador_de_testes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gerador_de_testesWebApp.Controllers
{
    [Route("questoes")]
    public class QuestaoController : Controller
    {
        private readonly GeradorDeTestesDbContext contexto;
        private readonly IRepositorioQuestao repositorioQuestao;
        private readonly IRepositorioTeste repositorioTeste;

        public QuestaoController(GeradorDeTestesDbContext contexto, IRepositorioQuestao repositorioQuestao, IRepositorioTeste repositorioTeste)
        {
            this.contexto = contexto;
            this.repositorioQuestao = repositorioQuestao;
            this.repositorioTeste = repositorioTeste;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var registros = repositorioQuestao.SelecionarRegistros();

            var visualizarVM = new VisualizarQuestaoViewModel(registros);

            return View(visualizarVM);
        }

        [HttpGet("cadastrar")]
        public IActionResult Cadastrar()
        {
            var cadastrarVM = new CadastrarQuestaoViewModel();

            return View(cadastrarVM);
        }

        [HttpPost("cadastrar")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(CadastrarQuestaoViewModel cadastrarVM)
        {
            var entidade = cadastrarVM.ParaEntidade();

            repositorioQuestao.AtualizarAlternativa(entidade.Alternativas.FirstOrDefault(a => a.Correta)!);

            var transacao = contexto.Database.BeginTransaction();

            try
            {
                repositorioQuestao.CadastrarRegistro(entidade);

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
            var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(id);

            var editarVM = new EditarQuestaoViewModel(
                id,
                registroSelecionado!.Enunciado,
                registroSelecionado.Materia,
                registroSelecionado.Alternativas
            );

            return View(editarVM);
        }

        [HttpPost("editar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Guid id, EditarQuestaoViewModel editarVM)
        {
            var entidadeEditada = editarVM.ParaEntidade();

            repositorioQuestao.AtualizarAlternativa(entidadeEditada.Alternativas.FirstOrDefault(a => a.Correta)!);

            var transacao = contexto.Database.BeginTransaction();

            try
            {
                repositorioQuestao.EditarRegistro(id, entidadeEditada);

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
            var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(id);

            var excluirVM = new ExcluirQuestaoViewModel(registroSelecionado!.Id, registroSelecionado.Enunciado);

            return View(excluirVM);
        }

        [HttpPost("excluir/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult ExcluirConfirmado(Guid id)
        {
            var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(id);
            var registrosTeste = repositorioTeste.SelecionarRegistros();
            foreach (var registro in registrosTeste)
            {
                foreach (var item in registro.QuestoesSelecionadas)
                {
                    if (item.Id == id)
                    {
                        ModelState.AddModelError("ExclusaoInvalida", "Não é possível excluir uma questão que possui testes associados.");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }

            var transacao = contexto.Database.BeginTransaction();

            try
            {
                repositorioQuestao.ExcluirRegistro(id);

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
        [HttpGet("detalhes/{id:guid}")]
        public IActionResult Detalhes(Guid id)
        {
            var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(id);

            var detalhesVM = new DetalhesQuestaoViewModel(registroSelecionado.Id,registroSelecionado.Enunciado, registroSelecionado.Materia, registroSelecionado.Alternativas);

            return View(detalhesVM);
        }
    }
}
