using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testes.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Gerador_de_testes.WebApp.Models;

    public class FormularioQuestaoViewModel
    {
        [Required(ErrorMessage = "O campo \"Enunciado\" é obrigatório.")]
        [MinLength(2, ErrorMessage = "O campo \"Enunciado\" precisa conter ao menos 2 caracteres.")]
        [MaxLength(100, ErrorMessage = "O campo \"Enunciado\" precisa conter no máximo 500 caracteres.")]
        public string? Enunciado { get; set; }

        [Required(ErrorMessage = "O campo \"Matéria\" é obrigatório.")]
        public Materia? Materia { get; set; }

        [Required(ErrorMessage = "O campo \"Alternativas\" é obrigatório.")]
        [MinLength(2, ErrorMessage = "O campo \"Alternativas\" precisa conter 2 alternativas no mínimo.")]
        [MaxLength(5, ErrorMessage = "O campo \"Alternativas\" precisa conter 5 alternativas no máximo.")]
        public List<Alternativa>? Alternativas { get; set; }
    }

    public class CadastrarQuestaoViewModel : FormularioQuestaoViewModel
    {
        public CadastrarQuestaoViewModel()
        {
            Materia = new Materia();
            Alternativas = new List<Alternativa>();
        }

        public CadastrarQuestaoViewModel(string enunciado) : this()
        {
            Enunciado = enunciado;
        }
    }

    public class EditarQuestaoViewModel : FormularioQuestaoViewModel
    {
        public Guid Id { get; set; }

        public EditarQuestaoViewModel()
        {
            
        }

        public EditarQuestaoViewModel(Guid id, string enunciado, Materia materia, List<Alternativa> alternativas) : this()
        {
            Id = id;
            Enunciado = enunciado;
            Materia = materia;
            Alternativas = alternativas;
        }
    }

    public class ExcluirQuestaoViewModel
    {
        public Guid Id { get; set; }
        public string Enunciado { get; set; }

        public ExcluirQuestaoViewModel(Guid id, string enunciado)
        {
            Id = id;
            Enunciado = enunciado;
        }
    }

    public class VisualizarQuestaoViewModel
    {
        public List<DetalhesQuestaoViewModel> Registros { get; set; }

        public VisualizarQuestaoViewModel(List<Questao> questoes)
        {
            Registros = new List<DetalhesQuestaoViewModel>();

            foreach (var q in questoes)
                Registros.Add(q.ParaDetalhesVM());
        }
    }

    public class DetalhesQuestaoViewModel
    {
        public Guid Id { get; set; }
        public string Enunciado { get; set; }
        public Materia Materia { get; set; }
        public List<Alternativa> Alternativas { get; set; }

        public DetalhesQuestaoViewModel(
            Guid id,
            string enunciado,
            Materia materia,
            List<Alternativa> alternativas
        )
        {
            Id = id;
            Enunciado = enunciado;
            Materia = materia;
            Alternativas = alternativas;
        }
    }

public class CadastrarAlternativaViewModel
{
    public Guid Id { get; set; }
    public string Resposta { get; set; }
    public Questao Questao { get; set; }
    public bool Correta { get; set; }
    public CadastrarAlternativaViewModel()
    {
        Questao = new Questao();
    }
    public CadastrarAlternativaViewModel(Guid id, string resposta, Questao questao, bool correta) : this()
    {
        Id = id;
        Resposta = resposta;
        Questao = questao;
        Correta = correta;
    }
}
    public class RemoverAlternativaViewModel
    {
        public Guid Id { get; set; }
    public RemoverAlternativaViewModel()
        {
        }
        public RemoverAlternativaViewModel(Guid id) : this()
        {
            Id = id;
        }
    }


