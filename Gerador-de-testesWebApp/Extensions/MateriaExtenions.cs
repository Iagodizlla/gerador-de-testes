using eAgenda.WebApp.Models;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;

namespace eAgenda.WebApp.Extensions;

public static class DespesaExtensions
{
    public static Materia ParaEntidade(this FormularioMateriaViewModel formularioVM, List<Disciplina> disciplinas)
    {
        Disciplina Selecionado = new Disciplina();
        foreach (var d in disciplinas)
        {
            if (d.Id.Equals(formularioVM.DisciplinaSelecionada))
            {
                Selecionado = d;
                break;
            }
        }
        return new Materia(
            formularioVM.Nome,
            formularioVM.Serie,
            Selecionado
        );
    }

    public static DetalhesMateriaViewModel ParaDetalhesVM(this Materia materia)
    {
        return new DetalhesMateriaViewModel(
                materia.Id,
                materia.Nome,
                materia.Serie,
                materia.Disciplina
        );
    }
}