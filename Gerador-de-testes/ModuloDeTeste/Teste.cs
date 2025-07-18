using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
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
        public List<Disciplina> Disciplina { get; set; }
        public string Serie { get; set; }
        public List<Materia> Materias { get; set; }
        public int QteQuestoes { get; set; }

        public Teste()
        {
            Materias = new List<Materia>();
            Disciplina = new List<Disciplina>();
        }

        public Teste(string titulo, List<Disciplina> disciplina, string serie, List<Materia> materias, int qteQuestoes  ) : this()
        {
            Titulo = titulo;
            Disciplina = disciplina;
            Serie = serie;
            Materias = materias;
            QteQuestoes = qteQuestoes;
        }

        public override void AtualizarRegistro(Teste registroEditado)
        {
            registroEditado.Titulo = Titulo;
            registroEditado.Disciplina = Disciplina;
            registroEditado.Serie = Serie;
            registroEditado.Materias = Materias;
            registroEditado.QteQuestoes = QteQuestoes;
        }
    }
}
