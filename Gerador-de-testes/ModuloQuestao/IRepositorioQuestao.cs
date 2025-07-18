namespace Gerador_de_testes.ModuloQuestao
{
    public interface IRepositorioQuestao
    {
        public void AdicionarAlternativa(Alternativa alternativa);
        public bool AtualizarAlternativa(Alternativa alternativaAtualizado);
        public bool RemoverAlternativa(Alternativa alternativa);
    }
}
