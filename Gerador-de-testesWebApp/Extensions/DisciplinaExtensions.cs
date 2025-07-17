using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.WebApp.Models;

namespace Gerador_de_testes.WebApp.Extensions;

public static class DisciplinaExtensions
{
    public static Disciplina ParaEntidade(this FormularioDisciplinaViewModel formularioVM)
    {
        return new Disciplina(formularioVM.Nome);
    }

    public static DetalhesDisciplinaViewModel ParaDetalhesVM(this Disciplina categoria)
    {
        return new DetalhesDisciplinaViewModel(
                categoria.Id,
                categoria.Nome
        );
    }
}