using Gerador_de_testes.ModuloQuestao;
using TesteFacil.Dominio.Compartilhado;
using TesteFacil.Dominio.ModuloMateria;

namespace TesteFacil.Dominio.ModuloQuestao;

public interface IRepositorioQuestao : IRepositorio<Questao>
{
    List<Questao> SelecionarQuestoesPorDisciplinaESerie(Guid disciplinaId, SerieMateria serie, int quantidadeQuestoes);
    List<Questao> SelecionarQuestoesPorMateria(Guid materiaId, int quantidadeQuestoes);
}