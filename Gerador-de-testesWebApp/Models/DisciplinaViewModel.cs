using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using System.ComponentModel.DataAnnotations;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;

namespace TesteFacil.WebApp.Models;

public abstract class FormularioDisciplinaViewModel
{
    [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Nome\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Nome\" precisa conter no máximo 100 caracteres.")]
    public string? Nome { get; set; }

    public static Disciplina ParaEntidade(FormularioDisciplinaViewModel viewModel)
    {
        return new Disciplina(viewModel.Nome ?? string.Empty);
    }
}

public class CadastrarDisciplinaViewModel : FormularioDisciplinaViewModel
{
    public CadastrarDisciplinaViewModel() { }

    public CadastrarDisciplinaViewModel(string nome) : this()
    {
        Nome = nome;
    }
}

public class EditarDisciplinaViewModel : FormularioDisciplinaViewModel
{
    public Guid Id { get; set; }

    public EditarDisciplinaViewModel() { }

    public EditarDisciplinaViewModel(Guid id, string nome) : this()
    {
        Id = id;
        Nome = nome;
    }
}

public class ExcluirDisciplinaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public ExcluirDisciplinaViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarDisciplinasViewModel
{
    public List<DetalhesDisciplinaViewModel> Registros { get; set; }

    public VisualizarDisciplinasViewModel(List<Disciplina> disciplinas)
    {
        Registros = new List<DetalhesDisciplinaViewModel>();

        foreach (var d in disciplinas)
        {
            var detalhesVm = DetalhesDisciplinaViewModel.ParaDetalhesVm(d);

            Registros.Add(detalhesVm);
        }
    }
}

public class DetalhesDisciplinaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public List<string> Materias { get; set; }

    public DetalhesDisciplinaViewModel(Guid id, string nome, List<Materia> materias)
    {
        Id = id;
        Nome = nome;
        Materias = materias.Select(m => m.Nome).ToList();
    }

    public static DetalhesDisciplinaViewModel ParaDetalhesVm(Disciplina disciplina)
    {
        return new DetalhesDisciplinaViewModel(disciplina.Id, disciplina.Nome, disciplina.Materias);
    }
}