using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
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
        public Disciplina Disciplina { get; set; }
        public string Serie { get; set; }

        // public List<Materias> Materias { get; set; }  Aguardando a implementação de Materias
        public int QteQuestoes { get; set; }

        public Teste()
        {
            // Aguardando a implementação de Materias
            //Materias = new List<Materias>();
        }

        public Teste(string titulo, Disciplina disciplina, string serie, /*List<Materias> materias,*/ int qteQuestoes  ) : this()
        {
            Titulo = titulo;
            Disciplina = disciplina;
            Serie = serie;
            // Aguardando a implementação de Materias
            // Materias = materias;
            QteQuestoes = qteQuestoes;
        }

        public override void AtualizarRegistro(Teste registroEditado)
        {
            registroEditado.Titulo = Titulo;
            registroEditado.Disciplina = Disciplina;
            registroEditado.Serie = Serie;
            // Aguardando a implementação de Materias
            // registroEditado.Materias = Materias;
            registroEditado.QteQuestoes = QteQuestoes;
        }
    }
}
