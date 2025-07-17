using Gerador_de_testes.Compartilhado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_de_testes.ModuloDeTestes
{
    public interface IRepositorioTestes : IRepositorio<Teste>
    {
        public List<Teste> SelecionarTestesPorDisciplina(Guid idDisciplina);
        public List<Teste> SelecionarTestesPorSerie(string serie);
        public List<Teste> SelecionarTestesPorTitulo(string titulo);
        public List<Teste> SelecionarTestesPorQuantidadeQuestoes(int quantidadeQuestoes);
    }
}
