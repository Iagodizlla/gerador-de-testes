using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using Gerador_de_testes.WebApp.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public List<SelectListItem> MateriasDisponiveis { get; set; } = new();
        public string? NomeMateria { get; set; }
        public class GerenciarAlternativasViewModel
        {
            public DetalhesQuestaoViewModel Questao { get; set; }
            public List<AlternativaQuestaoViewModel> Alternativas { get; set; }

            public GerenciarAlternativasViewModel() { }

            public GerenciarAlternativasViewModel(Questao questao) : this()
            {
                Questao = questao.ParaDetalhesVM();

                Alternativas = new List<AlternativaQuestaoViewModel>();

                foreach (var i in questao.Alternativas)
                {
                    var itemVM = new AlternativaQuestaoViewModel(i.Id, i.Resposta, i.Correta);

                    Alternativas.Add(itemVM);
                }
            }
        }

        public class AlternativaQuestaoViewModel
        {
            public Guid Id { get; set; }

            [Required(ErrorMessage = "O campo \"Resposta\" é obrigatório.")]
            public string Resposta { get; set; }
            public bool Correta { get; set; }

            public AlternativaQuestaoViewModel(Guid id, string resposta, bool correta)
            {
                Id = id;
                Resposta = resposta;
                Correta = correta;
            }
        }
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
        [Required(ErrorMessage = "O campo \"Resposta\" é obrigatório.")]
        public string? Resposta { get; set; }
        public bool Correta { get; set; }
        public Questao Questao { get; set; }
        public CadastrarAlternativaViewModel() { }
        public CadastrarAlternativaViewModel(string resposta, bool correta, Questao questao)
        {
            Resposta = resposta;
            Correta = correta;
            Questao = questao;
        }
}




