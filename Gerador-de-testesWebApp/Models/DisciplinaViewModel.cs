using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.WebApp.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Gerador_de_testes.WebApp.Models;

public class FormularioDisciplinaViewModel
{
    [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Nome\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Nome\" precisa conter no máximo 100 caracteres.")]
    public string? Nome { get; set; }
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
            Registros.Add(d.ParaDetalhesVM());
    }
}

public class DetalhesDisciplinaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public DetalhesDisciplinaViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}