using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloQuestao;
using Microsoft.EntityFrameworkCore;


namespace Gerador_de_testes.Infraestrutura.Orm.ModuloGestao
{
    public class RepositorioQuestaoEmOrm : RepositorioBaseEmOrm<Questao>, IRepositorioQuestao
    {
        private readonly DbSet<Questao> registros;
        public RepositorioQuestaoEmOrm(GeradorDeTestesDbContext contextoDados) : base(contextoDados)
        {
            this.registros = contextoDados.Set<Questao>();
        }

        public void AdicionarAlternativa(Alternativa alternativa)
        {
            
        }

        public bool AtualizarAlternativa(Alternativa alternativaAtualizado)
        {
            throw new NotImplementedException();
        }

        public bool RemoverAlternativa(Alternativa alternativa)
        {
            throw new NotImplementedException();
        }
    }
}
