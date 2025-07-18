using Gerador_de_testes.WebApp.Extensions;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Gerador_de_testes.WebApp.Models;

public class FormularioMateriaViewModel
{
    [Required(ErrorMessage = "O campo \"Nome\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Nome\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Nome\" precisa conter no máximo 100 caracteres.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "O campo \"Serie\" é obrigatório.")]
    [MinLength(2, ErrorMessage = "O campo \"Serie\" precisa conter ao menos 2 caracteres.")]
    [MaxLength(100, ErrorMessage = "O campo \"Serie\" precisa conter no máximo 50 caracteres.")]
    public string Serie { get; set; }

    [Required(ErrorMessage = "O campo \"Disciplina\" é obrigatório.")]
    public Guid? DisciplinaSelecionada { get; set; }
    public List<SelectListItem>? DisciplinasDisponiveis { get; set; }
}

public class CadastrarMateriaViewModel : FormularioMateriaViewModel
{
    public CadastrarMateriaViewModel()
    {
        DisciplinasDisponiveis = new List<SelectListItem>();
    }

    public CadastrarMateriaViewModel(List<Disciplina> disciplinasDisponiveis) : this()
    {
        foreach (var c in disciplinasDisponiveis)
        {
            var selecionarVM = new SelectListItem(c.Nome, c.Id.ToString());

            DisciplinasDisponiveis?.Add(selecionarVM);
        }
    }
}

public class EditarMateriaViewModel : FormularioMateriaViewModel
{
    public Guid Id { get; set; }

    public EditarMateriaViewModel()
    {
        DisciplinasDisponiveis = new List<SelectListItem>();
    }

    public EditarMateriaViewModel(
        Guid id,
        string nome,
        string serie,
        Disciplina disciplinaSelecionada,
        List<Disciplina> disciplinasDisponiveis
    ) : this()
    {
        Id = id;
        Nome = nome;
        Serie = serie;

        foreach (var cd in disciplinasDisponiveis)
        {
            var selecionarVM = new SelectListItem(cd.Nome, cd.Id.ToString());

            DisciplinasDisponiveis?.Add(selecionarVM);
        }
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
            Registros.Add(m.ParaDetalhesVM());
    }
}

public class DetalhesMateriaViewModel
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Serie { get; set; }

    public Disciplina Disciplina { get; set; }

    public DetalhesMateriaViewModel(
        Guid id,
        string nome,
        string serie,
        Disciplina disciplina
    )
    {
        Id = id;
        Nome = nome;
        Serie = serie;
        Disciplina = disciplina;
    }
}