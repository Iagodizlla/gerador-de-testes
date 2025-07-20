using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testesWebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Gerador_de_testesWebApp.Models
{
    public class FormularioTesteViewModel
    {
        [Required(ErrorMessage = "O campo \"Título\" é obrigatório.")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "O campo \"Disciplina\" é obrigatório.")]
        public Disciplina Disciplina { get; set; }

        [Required(ErrorMessage = "O campo \"Série\" é obrigatório.")]
        [StringLength(50, ErrorMessage = "A série deve ter no máximo 50 caracteres.")]
        public string Serie { get; set; }

        [Required(ErrorMessage = "O campo \"Matérias\" é obrigatório.")]
        [MinLength(1, ErrorMessage = "É necessário selecionar pelo menos uma matéria.")]
         public List<Materia> Materias { get; set; }

        [Required(ErrorMessage = "O campo \"Quantidade de Questões\" é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade de questões deve ser pelo menos 1.")]
        public int QteQuestoes { get; set; }
        public List<Questao> QuestoesSelecionadas { get; set; }
    }

    public class CadastrarTesteViewModel : FormularioTesteViewModel
    {
        public CadastrarTesteViewModel()
        {
            Disciplina = new Disciplina();

            Materias = new List<Materia>();

            QuestoesSelecionadas = new List<Questao>();
        }

        public CadastrarTesteViewModel(string titulo, Disciplina disciplina, string serie, List<Materia> materias, int qteQuestoes, List<Questao> questoesSelecionadas) : this()
        {
            Titulo = titulo;
            Disciplina = disciplina;
            Serie = serie;
            Materias = materias;
            QteQuestoes = qteQuestoes;
            QuestoesSelecionadas = questoesSelecionadas;
        }

    }

    public class EditarTesteViewModel : FormularioTesteViewModel
    {
        public Guid Id { get; set; }
        public EditarTesteViewModel() { }
        public EditarTesteViewModel(Guid id, string titulo, Disciplina disciplina, string serie, List<Materia> materias, int qteQuestoes, List<Questao> questoesSelecionadas) : this()
        {
            Id = id;
            Titulo = titulo;
            Disciplina = disciplina;
            Serie = serie;
            Materias = materias;
            QteQuestoes = qteQuestoes;
            QuestoesSelecionadas = questoesSelecionadas;
        }
    }


    public class ExcluirTesteViewModel
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public Disciplina Disciplina { get; set; }
        public string Serie { get; set; }
        public List<Materia> Materias { get; set; }
        public int QteQuestoes { get; set; }
        public List<Questao> QuestoesSelecionadas { get; set; }
        public ExcluirTesteViewModel() { }
        public ExcluirTesteViewModel(Teste teste)
        {

        }
    }

    public class VisualizarTesteViewModel
    {
        public List<DetalhesTesteViewModel> Registros { get; set; }

        public VisualizarTesteViewModel(List<Teste> compromissos)
        {
            Registros = new List<DetalhesTesteViewModel>();

            foreach (var c in compromissos)
                Registros.Add(c.ParaDetalhesVM());
        }
    }

    public class DetalhesTesteViewModel
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Disciplina { get; set; }
        public string Serie { get; set; }
        public string Materia { get; set; }
        public int QteQuestoes { get; set; }
        public List<Questao> QuestoesSelecionadas { get; set; }
        public DetalhesTesteViewModel() { }

        public DetalhesTesteViewModel(Teste teste)
        {
            Id = teste.Id.ToString();
            Titulo = teste.Titulo;
            //Disciplina = teste.Disciplina.Nome;
            Serie = teste.Serie;
            Materia = string.Join(", ", teste.Materias.Select(m => m.Nome));
            QteQuestoes = teste.QteQuestoes;
            QuestoesSelecionadas = teste.QuestoesSelecionadas;
    }
    }
}