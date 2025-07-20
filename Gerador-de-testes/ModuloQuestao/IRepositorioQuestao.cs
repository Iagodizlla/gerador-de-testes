using Gerador_de_testes.Compartilhado;

namespace Gerador_de_testes.ModuloQuestao
{
    public interface IRepositorioQuestao : IRepositorio<Questao>
    {
        public void AdicionarAlternativa(Alternativa alternativa, Guid IdQuestao);
        public bool AtualizarAlternativa(Alternativa alternativaAtualizado, Guid IdQuestao);
        public bool RemoverAlternativa(Alternativa alternativa, Guid IdQuestao);
    }
}
