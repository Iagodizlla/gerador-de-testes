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
            var registros = repositorioQuestao.SelecionarRegistros();

            var entidadeEditada = editarVM.ParaEntidade();

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
        [HttpGet("cadastrarAlternativa")]
        public IActionResult CadastrarAlternativa(Guid id)
        {
            var registroSelecionado = repositorioQuestao.SelecionarRegistroPorId(id);
            var cadastrarVM = new CadastrarAlternativaViewModel();

            return View(cadastrarVM);
        }

        [HttpPost("cadastrarAlternativa")]
        [ValidateAntiForgeryToken]
        public IActionResult CadastrarAlternativa(CadastrarAlternativaViewModel cadastrarVM)
        {
            var entidade = cadastrarVM.ParaEntidadeAlternativa();

            var transacao = contexto.Database.BeginTransaction();

            try
            {
                repositorioQuestao.AdicionarAlternativa(entidade, entidade.Questao.Id);

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
        [HttpGet("atualizarAlternativa/{id:guid}")]
        public IActionResult AtualizarAlternativa(Guid idAlternativa)
        {
            var AtualizarVM = new RemoverAlternativaViewModel(idAlternativa);

            return View(AtualizarVM);
        }

        [HttpPost("atualizarAlternativa/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult AtualizarAlternativa(RemoverAlternativaViewModel AtualizarVM)
        {
            var registro = repositorioQuestao.SelecionarAlternativa(AtualizarVM.Id);
            var transacao = contexto.Database.BeginTransaction();

            try
            {
                repositorioQuestao.AtualizarAlternativa(registro);

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
        [HttpGet("removerAlternativa/{id:guid}")]
        public IActionResult RemoverAlternativa(Guid idAlternativa)
        {
            var RemoverVM = new RemoverAlternativaViewModel(idAlternativa);

            return View(RemoverVM);
        }

        [HttpPost("removerAlternativa/{id:guid}")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoverAlternativa(RemoverAlternativaViewModel RemoverVM)
        {
            var registro = repositorioQuestao.SelecionarAlternativa(RemoverVM.Id);
            var transacao = contexto.Database.BeginTransaction();

            try
            {
                repositorioQuestao.RemoverAlternativa(registro);

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
}
