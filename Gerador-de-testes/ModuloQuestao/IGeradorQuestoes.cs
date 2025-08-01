using TesteFacil.Dominio.ModuloMateria;

namespace TesteFacil.Dominio.ModuloQuestao;

public interface IGeradorQuestoes
{
    public Task<List<Questao>> GerarQuestoesAsync(Materia materia, int quantidade);
}