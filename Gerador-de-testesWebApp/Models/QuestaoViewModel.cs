using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;

namespace TesteFacil.WebApp.Models;

public abstract class FormularioQuestaoViewModel
{
    [Required(ErrorMessage = "O campo \"Enunciado\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Enunciado\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(500, ErrorMessage = "O campo \"Enunciado\" precisa conter no máximo 500 caracteres.")]
    public string? Enunciado { get; set; }

    [Required(ErrorMessage = "O campo \"Matéria\" é obrigatório.")]
    public Guid MateriaId { get; set; }
    public List<SelectListItem> MateriasDisponiveis { get; set; } = new List<SelectListItem>();

    [MinLength(2, ErrorMessage = "O campo \"Alternativas\" precisa conter ao menos 2 itens.")]
    public List<AlternativaQuestaoViewModel> AlternativasSelecionadas { get; set; } = new List<AlternativaQuestaoViewModel>();

    public void AdicionarAlternativa(AdicionarAlternativaQuestaoViewModel alternativaVm)
    {
        var letraAlternativa = (char)('a' + AlternativasSelecionadas?.Count ?? 0);

        var alternativa = new AlternativaQuestaoViewModel(
            letraAlternativa,
            alternativaVm.Resposta,
            alternativaVm.Correta
        );

        AlternativasSelecionadas?.Add(alternativa);
    }

    public void RemoverAlternativa(AlternativaQuestaoViewModel alternativaVm)
    {
        if (!AlternativasSelecionadas.Contains(alternativaVm))
            return;

        AlternativasSelecionadas.Remove(alternativaVm);

        ReatribuirLetras();
    }

    private void ReatribuirLetras()
    {
        for (int i = 0; i < AlternativasSelecionadas.Count; i++)
            AlternativasSelecionadas[i].Letra = (char)('a' + i);
    }
}

public class CadastrarQuestaoViewModel : FormularioQuestaoViewModel
{
    public CadastrarQuestaoViewModel() { }

    public CadastrarQuestaoViewModel(List<Materia> materias) : this()
    {
        MateriasDisponiveis = materias
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();
    }

    public static Questao ParaEntidade(CadastrarQuestaoViewModel viewModel, List<Materia> materias)
    {
        Materia? materia = materias.Find(i => i.Id.Equals(viewModel.MateriaId));

        if (materia is null)
            throw new InvalidOperationException("A matéria requisitada selecionada não foi encontrada.");

        var questao = new Questao(viewModel.Enunciado ?? string.Empty, materia);

        if (viewModel.AlternativasSelecionadas is not null)
        {
            foreach (var a in viewModel.AlternativasSelecionadas)
                questao.AdicionarAlternativa(a.Resposta, a.Correta);
        }

        return questao;
    }
}

public class EditarQuestaoViewModel : FormularioQuestaoViewModel
{
    public Guid Id { get; set; }

    public EditarQuestaoViewModel() { }

    public EditarQuestaoViewModel(
        Guid id,
        string enunciado,
        Guid materiaId,
        List<Alternativa> alternativas,
        List<Materia> materias
    ) : this()
    {
        Id = id;
        Enunciado = enunciado;
        MateriaId = materiaId;

        AlternativasSelecionadas = alternativas
            .Select(a => new AlternativaQuestaoViewModel(a.Letra, a.Resposta, a.Correta))
            .ToList();

        MateriasDisponiveis = materias
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();
    }

    public static Questao ParaEntidade(EditarQuestaoViewModel viewModel, List<Materia> materias)
    {
        Materia? materia = materias.Find(i => i.Id.Equals(viewModel.MateriaId));

        if (materia is null)
            throw new InvalidOperationException("A matéria requisitada selecionada não foi encontrada.");

        var questao = new Questao(viewModel.Enunciado ?? string.Empty, materia);

        return questao;
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

public class VisualizarQuestoesViewModel
{
    public List<DetalhesQuestaoViewModel> Registros { get; set; }

    public VisualizarQuestoesViewModel(List<Questao> questoes)
    {
        Registros = new List<DetalhesQuestaoViewModel>();

        foreach (var q in questoes)
        {
            var detalhesVm = DetalhesQuestaoViewModel.ParaDetalhesVm(q);

            Registros.Add(detalhesVm);
        }
    }
}

public class DetalhesQuestaoViewModel
{
    public Guid Id { get; set; }
    public string Enunciado { get; set; }
    public string Materia { get; set; }
    public string UtilizadaEmTeste { get; set; }
    public string RespostaCorreta { get; set; }
    public List<AlternativaQuestaoViewModel> Alternativas { get; set; }

    public DetalhesQuestaoViewModel()
    {
        Alternativas = new List<AlternativaQuestaoViewModel>();
    }

    public DetalhesQuestaoViewModel(
        Guid id,
        string enunciado,
        string materia,
        string utilizadaEmTeste,
        string respostaCorreta,
        List<Alternativa> alternativas
    ) : this()
    {
        Id = id;
        Enunciado = enunciado;
        Materia = materia;
        UtilizadaEmTeste = utilizadaEmTeste;
        RespostaCorreta = respostaCorreta;

        Alternativas = alternativas
            .Select(a => new AlternativaQuestaoViewModel(a.Letra, a.Resposta, a.Correta))
            .ToList();
    }

    public static DetalhesQuestaoViewModel ParaDetalhesVm(Questao questao)
    {
        return new DetalhesQuestaoViewModel(
            questao.Id,
            questao.Enunciado,
            questao.Materia.Nome,
            questao.UtilizadaEmTeste ? "Sim" : "Não",
            questao.AlternativaCorreta?.Resposta ?? string.Empty,
            questao.Alternativas
        );
    }
}

public class AlternativaQuestaoViewModel
{
    public char Letra { get; set; }
    public string Resposta { get; set; }
    public bool Correta { get; set; }

    public AlternativaQuestaoViewModel() { }

    public AlternativaQuestaoViewModel(char letra, string resposta, bool correta) : this()
    {
        Letra = letra;
        Resposta = resposta;
        Correta = correta;
    }
}

public class AdicionarAlternativaQuestaoViewModel
{
    public string Resposta { get; set; }
    public bool Correta { get; set; }

    public AdicionarAlternativaQuestaoViewModel() { }

    public AdicionarAlternativaQuestaoViewModel(string resposta, bool correta) : this()
    {
        Resposta = resposta;
        Correta = correta;
    }
}

public class PrimeiraEtapaGerarQuestoesViewModel
{
    [Required(ErrorMessage = "O campo \"Matéria\" é obrigatório.")]
    public Guid MateriaId { get; set; }
    public List<SelectListItem> MateriasDisponiveis { get; set; } = new List<SelectListItem>();

    [Required(ErrorMessage = "O campo \"Quantidade de Questões\" é obrigatório.")]
    [Range(1, 100, ErrorMessage = "O campo \"Quantidade de Questões\" precisa conter um valor numérico entre 1 e 100.")]
    public int QuantidadeQuestoes { get; set; }

    public PrimeiraEtapaGerarQuestoesViewModel() { }

    public PrimeiraEtapaGerarQuestoesViewModel(List<Materia> materias) : this()
    {
        MateriasDisponiveis = materias
            .Select(m => new SelectListItem($"{m.Nome} - {m.Disciplina.Nome}", m.Id.ToString()))
            .ToList();
    }
}

public class SegundaEtapaGerarQuestoesViewModel
{
    public required Guid MateriaId { get; set; }
    public required string Materia { get; set; }

    public List<QuestaoGeradaViewModel> QuestoesGeradas { get; set; } = new List<QuestaoGeradaViewModel>();

    public SegundaEtapaGerarQuestoesViewModel() { }

    public SegundaEtapaGerarQuestoesViewModel(List<Questao> questoes) : this()
    {
        QuestoesGeradas = questoes
            .Select(QuestaoGeradaViewModel.ParaViewModel)
            .ToList();
    }

    public static List<Questao> ObterQuestoesGeradas(SegundaEtapaGerarQuestoesViewModel segundaEtapaVm, Materia materiaSelecionada)
    {
        var questoes = new List<Questao>();

        foreach (var questaoVm in segundaEtapaVm.QuestoesGeradas)
        {
            var questao = new Questao(questaoVm.Enunciado, materiaSelecionada);

            foreach (var alternativaVm in questaoVm.AlternativasGeradas)
                questao.AdicionarAlternativa(alternativaVm.Resposta, alternativaVm.Correta);

            questoes.Add(questao);
        }

        return questoes;
    }
}

public class QuestaoGeradaViewModel
{
    public required string Enunciado { get; set; }
    public required List<AlternativaQuestaoGeradaViewModel> AlternativasGeradas { get; set; } = new List<AlternativaQuestaoGeradaViewModel>();

    public static QuestaoGeradaViewModel ParaViewModel(Questao questao)
    {
        return new QuestaoGeradaViewModel
        {
            Enunciado = questao.Enunciado,
            AlternativasGeradas = questao.Alternativas
                .Select(AlternativaQuestaoGeradaViewModel.ParaViewModel)
                .ToList()
        };
    }
}

public class AlternativaQuestaoGeradaViewModel
{
    public required string Resposta { get; set; }
    public required bool Correta { get; set; }

    public static AlternativaQuestaoGeradaViewModel ParaViewModel(Alternativa alternativa)
    {
        return new AlternativaQuestaoGeradaViewModel
        {
            Resposta = alternativa.Resposta,
            Correta = alternativa.Correta
        };
    }
}