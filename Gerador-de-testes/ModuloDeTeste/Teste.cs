using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Gerador_de_testes.ModuloQuestao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_de_testes.ModuloDeTestes
{
    public class Teste : EntidadeBase<Teste>
    {
        public string Titulo { get; set; }
        public List<Disciplina> Disciplinas { get; set; }
        public List<Questao> QuestoesSelecionadas { get; set; }
        public string Serie { get; set; }
        public List<Materia> Materias { get; set; }
        public int QteQuestoes { get; set; }

        public Teste()
        {
            Materias = new List<Materia>();
            Disciplinas = new List<Disciplina>();
            QuestoesSelecionadas = new List<Questao>();
        }

        public Teste(string titulo, List<Disciplina> disciplina, string serie, List<Materia> materias, int qteQuestoes, List<Questao> questoesSelecionadas) : this()
        {
            Titulo = titulo;
            Disciplinas = disciplina;
            Serie = serie;
            Materias = materias;
            QteQuestoes = qteQuestoes;
            QuestoesSelecionadas = questoesSelecionadas;
        }

        public override void AtualizarRegistro(Teste registroEditado)
        {
            registroEditado.Titulo = Titulo;
            registroEditado.Disciplinas = Disciplinas;
            registroEditado.Serie = Serie;
            registroEditado.Materias = Materias;
            registroEditado.QteQuestoes = QteQuestoes;
            registroEditado.QuestoesSelecionadas = QuestoesSelecionadas;
        }
    }
}
