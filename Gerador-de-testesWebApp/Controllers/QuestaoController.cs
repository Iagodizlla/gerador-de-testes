using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testes.WebApp.Extensions;
using Gerador_de_testes.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Runtime.Serialization;
namespace Gerador_de_testesWebApp.Controllers
{
    [Route("questoes")]
    public class QuestaoController : Controller
    {
        private readonly GeradorDeTestesDbContext contexto;
        private readonly IRepositorioQuestao repositorioQuestao;
        private readonly IRepositorioTeste repositorioTeste;
        private readonly IRepositorioMateria repositorioMateria;

        public QuestaoController(GeradorDeTestesDbContext contexto, IRepositorioQuestao repositorioQuestao, IRepositorioTeste repositorioTeste, IRepositorioMateria repositorioMateria)
        {
            this.contexto = contexto;
            this.repositorioQuestao = repositorioQuestao;
            this.repositorioTeste = repositorioTeste;
            this.repositorioMateria = repositorioMateria;
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
            List<Materia> materias = repositorioMateria.SelecionarRegistros();
            foreach (var item in materias)
            {
                cadastrarVM.MateriasDisponiveis.Add(new SelectListItem
                {
                    Text = item.Nome
                });
            }


            return View(cadastrarVM);
        }

        [HttpPost("cadastrar")]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastrar(CadastrarQuestaoViewModel cadastrarVM)
        {
            var materias = repositorioMateria.SelecionarRegistros();
            
            if (string.IsNullOrWhiteSpace(cadastrarVM.NomeMateria))
            {
                ModelState.AddModelError("NomeMateria", "Não é possivel adicionar uma questão sem uma matéria.");
            }
               
            if (cadastrarVM.AlternativaCorretaIndice == null || cadastrarVM.AlternativaCorretaIndice < 0 || cadastrarVM.AlternativaCorretaIndice >= cadastrarVM.AlternativasRespostas.Count)
            {
                ModelState.AddModelError("AlternativaCorretaIndice", "Não é possivel adicionar uma questão sem uma alternativa correta.");
            }
            if (cadastrarVM.AlternativasRespostas == null || cadastrarVM.AlternativasRespostas.Count < 2 )
            {
                ModelState.AddModelError("AlternativasRespostas", "É necessário haver pelo menos 2 alternativas.");
            }
            if (!ModelState.IsValid)
            {
                foreach (var item in materias)
                {
                    cadastrarVM.MateriasDisponiveis.Add(new SelectListItem
                    {
                        Text = item.Nome
                    });
                }
                return View(cadastrarVM);
            }
            var entidade = cadastrarVM.ParaEntidade();
            entidade.Materia = materias.FirstOrDefault(m => m.Nome == cadastrarVM.NomeMateria)!;
            entidade.Alternativas = new List<Alternativa>();
            for (int i = 0; i < cadastrarVM.AlternativasRespostas!.Count; i++)
            {
                if (i == cadastrarVM.AlternativaCorretaIndice)
                {
                    Alternativa alternativa = new Alternativa(cadastrarVM.AlternativasRespostas[i], true, entidade);
                    entidade.Alternativas.Add(alternativa);
                }
                else
                {
                    Alternativa alternativa = new Alternativa(cadastrarVM.AlternativasRespostas[i], false, entidade);
                    entidade.Alternativas.Add(alternativa);
                }

            }

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
            List<string> lista = new List<string>();
            foreach (var item in registroSelecionado!.Alternativas)
            {
                lista.Add(item.Resposta);
            }
            

            var editarVM = new EditarQuestaoViewModel(
                id,
                registroSelecionado!.Enunciado,
                lista

            );
            List<Materia> materias = repositorioMateria.SelecionarRegistros();
            foreach (var item in materias)
            {
                editarVM.MateriasDisponiveis.Add(new SelectListItem
                {
                    Text = item.Nome
                });
            }
            editarVM.AlternativaCorretaIndice = registroSelecionado.Alternativas
            .FindIndex(a => a.Correta == true);

            return View(editarVM);
        }

        [HttpPost("editar/{id:guid}")]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Guid id, EditarQuestaoViewModel editarVM)
        {
            
            var materias = repositorioMateria.SelecionarRegistros();

            if (string.IsNullOrWhiteSpace(editarVM.NomeMateria))
            {
                ModelState.AddModelError("NomeMateria", "Não é possivel adicionar uma questão sem uma matéria.");
            }
            if (editarVM.AlternativasRespostas == null || editarVM.AlternativasRespostas.Count < 2)
            {
                ModelState.AddModelError("AlternativasRespostas", "É necessário haver pelo menos 2 alternativas.");
            }
            if (editarVM.AlternativaCorretaIndice == null || editarVM.AlternativaCorretaIndice < 0 || editarVM.AlternativaCorretaIndice >= editarVM.AlternativasRespostas!.Count)
            {
                ModelState.AddModelError("AlternativaCorretaIndice", "Não é possivel adicionar uma questão sem uma alternativa correta.");
            }
            
            if (!ModelState.IsValid)
            {
                foreach (var item in materias)
                {
                    editarVM.MateriasDisponiveis.Add(new SelectListItem
                    {
                        Text = item.Nome
                    });
                }
                return View(editarVM);
            }
            var entidadeEditada = editarVM.ParaEntidade();
            entidadeEditada.Materia = materias.FirstOrDefault(m => m.Nome == editarVM.NomeMateria)!;
            entidadeEditada.Alternativas = new List<Alternativa>();
            for (int i = 0; i < editarVM.AlternativasRespostas!.Count; i++)
            {
                if (i == editarVM.AlternativaCorretaIndice)
                {
                    Alternativa alternativa = new Alternativa(editarVM.AlternativasRespostas[i], true, entidadeEditada);
                    entidadeEditada.Alternativas.Add(alternativa);
                }
                else
                {
                    Alternativa alternativa = new Alternativa(editarVM.AlternativasRespostas[i], false, entidadeEditada);
                    entidadeEditada.Alternativas.Add(alternativa);
                }

            }
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
            var possuiTestesAssociados = contexto.Testes.Any(q => q.QuestoesSelecionadas.Any(d => d.Id == id));

            if (possuiTestesAssociados)
            {
                ModelState.AddModelError("ExclusaoInvalida", "Não é possível excluir uma Questao que possui testes associados.");
                return RedirectToAction(nameof(Index));
            }

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

            if (registroSelecionado == null)
                return NotFound();

            var detalhesVM = new GerenciarAlternativasViewModel(registroSelecionado!);

            return View(detalhesVM);
        }
    }
}
