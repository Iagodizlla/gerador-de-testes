using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TesteFacil.Dominio.Extensions;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;

namespace TesteFacil.WebApp.Models;

public class PrimeiraEtapaGerarTesteViewModel
{
    [Required(ErrorMessage = "O campo \"Título\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Título\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Título\" precisa conter no máximo 100 caracteres.")]
    public string Titulo { get; set; }

    [Required(ErrorMessage = "O campo \"Disciplina\" é obrigatório.")]
    public Guid DisciplinaId { get; set; }
    public required List<SelectListItem>? DisciplinasDisponiveis { get; set; } = new List<SelectListItem>();

    [Required(ErrorMessage = "O campo \"Série da Matéria\" é obrigatório.")]
    public SerieMateria Serie { get; set; }
    public List<SelectListItem>? SeriesMateriasDisponiveis { get; set; } = new List<SelectListItem>();

    [Required(ErrorMessage = "O campo \"Quantidade de Questões\" é obrigatório.")]
    [Range(1, 100, ErrorMessage = "O campo \"Quantidade de Questões\" precisa conter um valor numérico entre 1 e 100.")]
    public int QuantidadeQuestoes { get; set; }

    public bool Recuperacao { get; set; }

    public static SegundaEtapaGerarTesteViewModel AvancarEtapa(
        PrimeiraEtapaGerarTesteViewModel primeiraEtapaVm,
        Disciplina disciplinaSelecionada,
        List<Materia> materiasFiltradas
    )
    {
        return new SegundaEtapaGerarTesteViewModel
        {
            Titulo = primeiraEtapaVm.Titulo ?? string.Empty,

            DisciplinaId = primeiraEtapaVm.DisciplinaId,
            Disciplina = disciplinaSelecionada.Nome,

            Serie = primeiraEtapaVm.Serie,
            NomeSerie = primeiraEtapaVm.Serie.GetDisplayName() ?? primeiraEtapaVm.Serie.ToString(),
            QuantidadeQuestoes = primeiraEtapaVm.QuantidadeQuestoes,
            Recuperacao = primeiraEtapaVm.Recuperacao,

            MateriasDisponiveis = materiasFiltradas
                .Select(m => new SelectListItem(m.Nome, m.Id.ToString()))
                .ToList()
        };
    }
}

public class SegundaEtapaGerarTesteViewModel
{
    public required string Titulo { get; set; }

    public required Guid DisciplinaId { get; set; }
    public required string Disciplina { get; set; }

    public required SerieMateria Serie { get; set; }
    public required string NomeSerie { get; set; }

    public required int QuantidadeQuestoes { get; set; }
    public required bool Recuperacao { get; set; }

    public Guid? MateriaId { get; set; }
    public List<SelectListItem>? MateriasDisponiveis { get; set; } = new List<SelectListItem>();

    public List<DetalhesQuestaoViewModel> QuestoesSorteadas { get; set; } = new List<DetalhesQuestaoViewModel>();

    public static Teste ParaEntidade(
        SegundaEtapaGerarTesteViewModel segundaEtapaVm,
        List<Disciplina> disciplinas,
        List<Materia> materias,
        List<Questao> questoes
    )
    {
        var disciplina = disciplinas.Find(d => d.Id.Equals(segundaEtapaVm.DisciplinaId));

        if (disciplina is null)
            throw new InvalidOperationException("A disciplina requisitada não foi encontrada no sistema.");

        var materia = materias.Find(d => d.Id.Equals(segundaEtapaVm.MateriaId));

        var idsQuestoesSelecionadas = segundaEtapaVm.QuestoesSorteadas
            .Select(q => q.Id)
            .ToHashSet();

        var questoesSelecionadas = questoes
            .Where(q => idsQuestoesSelecionadas.Contains(q.Id))
            .ToList();

        return new Teste(
            segundaEtapaVm.Titulo,
            segundaEtapaVm.Recuperacao,
            segundaEtapaVm.QuantidadeQuestoes,
            segundaEtapaVm.Serie,
            disciplina,
            materia,
            questoesSelecionadas
        );
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

public class DuplicarTesteViewModel
{
    public required Guid TesteId { get; set; }

    [Required(ErrorMessage = "O campo \"Título\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Título\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Título\" precisa conter no máximo 100 caracteres.")]
    public string Titulo { get; set; }

    public required Guid DisciplinaId { get; set; }
    public required string Disciplina { get; set; }

    public required SerieMateria Serie { get; set; }
    public required string NomeSerie { get; set; }

    public required int QuantidadeQuestoes { get; set; }
    public required bool Recuperacao { get; set; }

    public Guid? MateriaId { get; set; }
    public List<SelectListItem>? MateriasDisponiveis { get; set; } = new List<SelectListItem>();

    public List<DetalhesQuestaoViewModel> QuestoesSorteadas { get; set; } = new List<DetalhesQuestaoViewModel>();

    public static Teste ParaEntidade(
        DuplicarTesteViewModel segundaEtapaVm,
        List<Disciplina> disciplinas,
        List<Materia> materias,
        List<Questao> questoes
    )
    {
        var disciplina = disciplinas.Find(d => d.Id.Equals(segundaEtapaVm.DisciplinaId));

        if (disciplina is null)
            throw new InvalidOperationException("A disciplina requisitada não foi encontrada no sistema.");

        var materia = materias.Find(d => d.Id.Equals(segundaEtapaVm.MateriaId));

        var idsQuestoesSelecionadas = segundaEtapaVm.QuestoesSorteadas
            .Select(q => q.Id)
            .ToHashSet();

        var questoesSelecionadas = questoes
            .Where(q => idsQuestoesSelecionadas.Contains(q.Id))
            .ToList();

        return new Teste(
            segundaEtapaVm.Titulo,
            segundaEtapaVm.Recuperacao,
            segundaEtapaVm.QuantidadeQuestoes,
            segundaEtapaVm.Serie,
            disciplina,
            materia,
            questoesSelecionadas
        );
    }
}


public class VisualizarTestesViewModel
{
    public List<DetalhesTesteViewModel> Registros { get; set; }

    public VisualizarTestesViewModel(List<Teste> testes)
    {
        Registros = testes
            .Select(DetalhesTesteViewModel.ParaDetalhesVm)
            .ToList();
    }
}

public class DetalhesTesteViewModel
{
    public required Guid Id { get; set; }
    public required string DataGeracao { get; set; }
    public required string Titulo { get; set; }
    public required string Disciplina { get; set; }
    public required string? Materia { get; set; }
    public required string Serie { get; set; }
    public required string Recuperacao { get; set; }
    public required List<DetalhesQuestaoViewModel> QuestoesSorteadas { get; set; }

    public static DetalhesTesteViewModel ParaDetalhesVm(Teste teste)
    {
        return new DetalhesTesteViewModel
        {
            Id = teste.Id,
            DataGeracao = teste.DataGeracao.ToShortDateString(),
            Titulo = teste.Titulo,
            Recuperacao = teste.Recuperacao ? "Sim" : "Não",
            Disciplina = teste.Disciplina.Nome,
            Materia = teste.Materia?.Nome,
            Serie = teste.Serie.GetDisplayName() ?? teste.Serie.ToString(),
            QuestoesSorteadas = teste.Questoes
                .Select(q => new DetalhesQuestaoViewModel(
                    q.Id,
                    q.Enunciado,
                    q.Materia.Nome,
                    q.UtilizadaEmTeste ? "Sim" : "Não",
                    q.AlternativaCorreta?.Resposta ?? string.Empty,
                    q.Alternativas
                ))
                .ToList()
        };
    }
}