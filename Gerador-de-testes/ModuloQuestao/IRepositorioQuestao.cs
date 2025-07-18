namespace Gerador_de_testes.ModuloQuestao
{
    public interface IRepositorioQuestao
    {
        public void CadastrarRegistro(Questao Questao);
        public bool EditarRegistro(Guid idQuestao, Questao tarefaEditada);
        public bool ExcluirRegistro(Guid idQuestao);
        public void AdicionarAlternativa(Alternativa alternativa);
        public bool AtualizarAlternativa(Alternativa alternativaAtualizado);
        public bool RemoverAlternativa(Alternativa alternativa);
        public List<Questao> SelecionarRegistros();
        public Questao? SelecionarRegistroPorId(Guid idQuestao);
    }
}
