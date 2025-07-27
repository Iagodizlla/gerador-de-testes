using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;

namespace TesteFacil.WebApp.Models;

public abstract class FormularioMateriaViewModel
{
    [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Nome\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Nome\" precisa conter no máximo 100 caracteres.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O campo \"Série\" é obrigatório.")]
    public SerieMateria Serie { get; set; }

    [Required(ErrorMessage = "O campo \"Disciplina\" é obrigatório.")]
    public Guid DisciplinaId { get; set; }

    public List<SelectListItem>? DisciplinasDisponiveis { get; set; } = new List<SelectListItem>();

    public static Materia ParaEntidade(FormularioMateriaViewModel viewModel, List<Disciplina> disciplinas)
    {
        Disciplina? disciplina = disciplinas.Find(i => i.Id.Equals(viewModel.DisciplinaId));

        if (disciplina is null)
            throw new ArgumentNullException("A disciplina requisitada não foi encontrada no sistema.");

        return new Materia(
            viewModel.Nome ?? string.Empty,
            viewModel.Serie,
            disciplina
        );
    }
}

public class CadastrarMateriaViewModel : FormularioMateriaViewModel
{
    public CadastrarMateriaViewModel() { }

    public CadastrarMateriaViewModel(List<Disciplina> disciplinas) : this()
    {
        DisciplinasDisponiveis = disciplinas
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();
    }
}

public class EditarMateriaViewModel : FormularioMateriaViewModel
{
    public Guid Id { get; set; }

    public EditarMateriaViewModel() { }

    public EditarMateriaViewModel(
        Guid id,
        string nome,
        SerieMateria serie,
        Guid disciplinaId,
        List<Disciplina> disciplinas
    ) : this()
    {
        Id = id;
        Nome = nome;
        Serie = serie;
        DisciplinaId = disciplinaId;

        DisciplinasDisponiveis = disciplinas
            .Select(d => new SelectListItem(d.Nome, d.Id.ToString()))
            .ToList();
    }
}

public class ExcluirMateriaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }

    public ExcluirMateriaViewModel(Guid id, string nome)
    {
        Id = id;
        Nome = nome;
    }
}

public class VisualizarMateriasViewModel
{
    public List<DetalhesMateriaViewModel> Registros { get; set; }

    public VisualizarMateriasViewModel(List<Materia> materias)
    {
        Registros = new List<DetalhesMateriaViewModel>();

        foreach (var m in materias)
        {
            var detalhesVm = DetalhesMateriaViewModel.ParaDetalhesVm(m);

            Registros.Add(detalhesVm);
        }
    }
}

public class DetalhesMateriaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public SerieMateria Serie { get; set; }
    public string Disciplina { get; set; }

    public DetalhesMateriaViewModel(Guid id, string nome, SerieMateria serie, string disciplina)
    {
        Id = id;
        Nome = nome;
        Serie = serie;
        Disciplina = disciplina;
    }

    public static DetalhesMateriaViewModel ParaDetalhesVm(Materia materia)
    {
        return new DetalhesMateriaViewModel(
            materia.Id,
            materia.Nome,
            materia.Serie,
            materia.Disciplina.Nome
        );
    }
}