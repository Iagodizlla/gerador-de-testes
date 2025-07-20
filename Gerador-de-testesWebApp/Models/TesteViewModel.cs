using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testesWebApp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Gerador_de_testesWebApp.Models
{
    public class FormularioTesteViewModel
    {
        [Required(ErrorMessage = "O campo \"Título\" é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O campo \"Série\" é obrigatório.")]
        [StringLength(50, ErrorMessage = "A série deve ter no máximo 50 caracteres.")]
        public string Serie { get; set; }

        [Required(ErrorMessage = "O campo \"Quantidade de Questões\" é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade de questões deve ser pelo menos 1.")]
        public int QteQuestoes { get; set; }

        public Guid DisciplinaId { get; set; }
        public List<SelectListItem> Disciplinas { get; set; }

        public Guid MateriaId { get; set; }
        public List<SelectListItem> Materias { get; set; }

        public List<Questao> QuestoesSelecionadas { get; set; }
    }

    public class CadastrarTesteViewModel : FormularioTesteViewModel
    {
        public CadastrarTesteViewModel()
        {
            Disciplinas = new List<SelectListItem>();
            Materias = new List<SelectListItem>();
        }

        public CadastrarTesteViewModel(List<Disciplina> disciplinas, List<Materia> materias) : this()
        {
            foreach(var d in disciplinas)
            {
                var selecionarVM = new SelectListItem(d.Nome, d.Id.ToString());
                Disciplinas.Add(selecionarVM);
            }

            foreach (var m in materias)
            {
                var selecionarVM = new SelectListItem(m.Nome, m.Id.ToString());
                Materias.Add(selecionarVM);
            }
        }


    }

    public class EditarTesteViewModel : FormularioTesteViewModel
    {
        Guid Id { get; set; }
        public Guid DisciplinaId { get; set; }
        public Guid MateriaId { get; set; }

        public EditarTesteViewModel()
        {
            Disciplinas = new List<SelectListItem>();
            Materias = new List<SelectListItem>();
        }

        public EditarTesteViewModel(
                Guid id,
                string titulo,
                string serie,
                int qteQuestoes,
                List<Disciplina> disciplinas,
                List<Materia> materias,
                List<Questao> questoesSelecionadas
                ): this()
        {
            Id = id;
            Titulo = titulo;
            Serie = serie;
            QteQuestoes = qteQuestoes;
            Disciplinas = new List<SelectListItem>();
            foreach (var d in disciplinas)
            {
                var selecionarVM = new SelectListItem(d.Nome, d.Id.ToString());
                Disciplinas.Add(selecionarVM);
            }
            Materias = new List<SelectListItem>();
            foreach (var m in materias)
            {
                var selecionarVM = new SelectListItem(m.Nome, m.Id.ToString());
                Materias.Add(selecionarVM);
            }
            QuestoesSelecionadas = questoesSelecionadas;
        }

    }


    public class ExcluirTesteViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }

        public ExcluirTesteViewModel(Guid id, string titulo)
        {
            Id = id;
            Titulo = titulo;
        }
    }

    public class VisualizarTesteViewModel
    {
        public List<DetalhesTesteViewModel> Registros { get; set; }

        public VisualizarTesteViewModel(List<Teste> testes)
        {
            Registros = new List<DetalhesTesteViewModel>();

            foreach (var c in testes)
                Registros.Add(c.ParaDetalhesVM());
        }
    }

    public class DetalhesTesteViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Serie { get; set; }
        public int QteQuestoes { get; set; }
        public List<Disciplina> Disciplinas { get; set; }
        public List<Materia> Materias { get; set; }
        public List<Questao> QuestoesSelecionadas { get; set; }

        public DetalhesTesteViewModel(
            Guid id,
            string titulo,
            string serie,
            int qteQuestoes,
            List<Disciplina> disciplinas,
            List<Materia> materias,
            List<Questao> questoesSelecionadas)
        {
            Id = id;
            Titulo = titulo;
            Serie = serie;
            QteQuestoes = qteQuestoes;
            Disciplinas = disciplinas;
            Materias = materias;
            QuestoesSelecionadas = questoesSelecionadas;
        }

    }
}