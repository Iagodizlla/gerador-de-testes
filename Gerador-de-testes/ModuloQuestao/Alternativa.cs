using Gerador_de_testes.ModuloMateria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_de_testes.ModuloQuestao
{
    public class Alternativa
    {
        public Guid Id { get; set; }
        public string Resposta { get; set; }
        public Questao Questao { get; set; }
        public bool Correta { get; set; }
        public Alternativa() { }
        public Alternativa(string resposta, bool correta, Questao questao)
        {
            Id = Guid.NewGuid();
            Resposta = resposta;
            Correta = correta;
            Questao = new Questao();
        }
    }
   
}


