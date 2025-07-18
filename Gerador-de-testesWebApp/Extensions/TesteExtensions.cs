using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.WebApp.Models;
using Gerador_de_testesWebApp.Models;

namespace Gerador_de_testesWebApp.Extensions
{
    public static class TesteExtensions
    {
        public static Teste ParaEntidade(this FormularioTesteViewModel formularioVM)
        {
            return new Teste(
                formularioVM.Titulo,
                formularioVM.Disciplina,
                formularioVM.Serie,
                // Aguardando a implementação de Materias
                // formularioVM.Materias,
                formularioVM.QteQuestoes
                );
        }

        public static DetalhesTesteViewModel ParaDetalhesVM(this Teste teste)
        {
            return new DetalhesTesteViewModel
            {
                Id = teste.Id.ToString(),
                Titulo = teste.Titulo,
                Disciplina = teste.Disciplina.ToString(),
                Serie = teste.Serie,
                // Aguardando a implementação de Materias
                // Materias = teste.Materias,
                QteQuestoes = teste.QteQuestoes
            }; 
        }
    }
}
