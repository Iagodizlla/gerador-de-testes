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
            registros = contextoDados.Set<Questao>();
        }
        public void AdicionarAlternativa(Alternativa alternativa, Guid IdQuestao)
        {
            var registro = SelecionarRegistroPorId(IdQuestao)!;
            registro.Alternativas.Add(alternativa);
        }
        public bool AtualizarAlternativa(Alternativa alternativa)
        {
            var registro = alternativa.Questao;
            if(registro is null) return false;

            registro.Alternativas.ForEach(a => a.Correta = false);

            var alternativaCorreta = registro.Alternativas
            .FirstOrDefault(a => a.Id == alternativa.Id);
            if (alternativaCorreta is null) return false;
            alternativaCorreta.Correta = true;
            return true;
        }

        public bool RemoverAlternativa(Alternativa alternativa)
        {
            var registro = alternativa.Questao;
            registro.Alternativas.Remove(alternativa);
            return true;
        }

        public Alternativa SelecionarAlternativa(Guid idAlternativa)
        {
            foreach (var questoes in SelecionarRegistros())
            {
                foreach(var alternativa in questoes.Alternativas)
                {
                    if(alternativa.Id == idAlternativa)
                        return alternativa;
                }
            }
            return null;
        }

        public List<Alternativa> SelecionarTodasAlternativasDaQuestao(Questao questao)
        {
            return questao.Alternativas;
        }
    }
}
