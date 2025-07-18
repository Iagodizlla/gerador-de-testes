using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDeTestes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_de_testes.Infraestrutura.Orm.ModuloDeTestes
{
    public class RepositorioTestesEmOrm : RepositorioBaseEmOrm<Teste>, IRepositorioTeste
    {
        public RepositorioTestesEmOrm(GeradorDeTestesDbContext contexto) : base(contexto)
        {
        }

        public List<Teste> SelecionarTestesPorDisciplina(Guid idDisciplina)
        {
            throw new NotImplementedException();
        }

        public List<Teste> SelecionarTestesPorQuantidadeQuestoes(int quantidadeQuestoes)
        {
            throw new NotImplementedException();
        }

        public List<Teste> SelecionarTestesPorSerie(string serie)
        {
            throw new NotImplementedException();
        }

        public List<Teste> SelecionarTestesPorTitulo(string titulo)
        {
            throw new NotImplementedException();
        }
    }
}
