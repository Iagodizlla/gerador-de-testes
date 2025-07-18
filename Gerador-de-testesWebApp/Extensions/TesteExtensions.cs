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
            return new Teste
            {
                Titulo = formularioVM.Titulo,
                Disciplina = formularioVM.Disciplina,
                Serie = formularioVM.Serie,
                Materias = formularioVM.Materias,
                QteQuestoes = formularioVM.QteQuestoes
            };
        }

        public static DetalhesTesteViewModel ParaDetalhesVM(this Teste teste)
        {
            return new DetalhesTesteViewModel
            {
                Id = teste.Id.ToString(),
                Titulo = teste.Titulo,
                Disciplina = teste.Disciplina.ToString(),
                Serie = teste.Serie,
                Materia = teste.Materias.ToString(),
                QteQuestoes = teste.QteQuestoes
            }; 
        }
    }
}
