using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.WebApp.Models;
using Gerador_de_testesWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gerador_de_testesWebApp.Extensions
{
    public static class TesteExtensions
    {
        public static Teste ParaEntidade(this FormularioTesteViewModel formularioVM, List<Materia> materias, List<Disciplina> disciplinas)
        {
            Materia? materiaSelecionada = null;
            foreach (var m in materias)
            {
                if (m.Id.Equals(formularioVM.MateriaId))
                {
                    materiaSelecionada = m;
                    break;
                }
            }

            Disciplina? disciplinaSelecionada = null;
            foreach (var d in disciplinas)
            {
                if (d.Id.Equals(formularioVM.DisciplinaId))
                {
                    disciplinaSelecionada = d;
                    break;
                }
            }

            return new Teste(
                formularioVM.Titulo,
                formularioVM.Disciplinas.Select(d => disciplinas.FirstOrDefault(x => x.Id.Equals(d.Value))).ToList()!,
                formularioVM.Serie,
                materias.Where(m => m.Id.Equals(formularioVM.MateriaId)).ToList(),
                formularioVM.QteQuestoes,
                formularioVM.QuestoesSelecionadas
                );

        }

        public static DetalhesTesteViewModel ParaDetalhesVM(this Teste teste)
        {
            //return new DetalhesTesteViewModel(
            //    teste.Id,
            //    teste.Titulo,
            //    teste.Serie,
            //    teste.QteQuestoes,



            //    );

        }
    }
}
