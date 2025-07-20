using Gerador_de_testes.Compartilhado;

namespace Gerador_de_testes.ModuloQuestao
{
    public interface IRepositorioQuestao : IRepositorio<Questao>
    {
        public void AdicionarAlternativa(Alternativa alternativa, Guid IdQuestao);
        public bool AtualizarAlternativa(Alternativa alternativaAtualizado);
        public bool RemoverAlternativa(Alternativa alternativa);
        public Alternativa SelecionarAlternativa(Guid idAlternativa);
        public List<Alternativa> SelecionarTodasAlternativasDaQuestao(Questao questao);
    }
}
