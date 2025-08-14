using TesteFacil.Aplicacao.ModuloDisciplina;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;

namespace TesteFacil.Testes.Unidade.ModuloDisiplina;

[TestClass]
[TestCategory("Testes de Unidade de Disciplina")]
public sealed class DisciplinaTests
{
    private Disciplina? disciplina;

    [TestMethod]
    public void Deve_AdicionarMateria_ADisciplina_Corretamente()
    {
        // Arrange
        disciplina = new Disciplina("Matemática");

        var materia = new Materia("Quatro Operações", SerieMateria.PrimeiraSerie, disciplina);

        // Act
        disciplina.AdicionarMateria(materia);

        // Assert
        var disciplinaContemMateria = disciplina.Materias.Contains(materia);

        Assert.IsTrue(disciplinaContemMateria);
    }

    [TestMethod]
    public void Deve_SortearQuestoes_DaDisciplina_Corretamente()
    {
        // Arrange
        disciplina = new Disciplina("Matemática");

        var materiaQuatroOperacoes = new Materia(
            "Quatro Operações",
            SerieMateria.SegundaSerie,
            disciplina
        );

        var materiaFracoes = new Materia(
            "Frações",
            SerieMateria.SegundaSerie,
            disciplina
        );

        materiaQuatroOperacoes.AdicionarQuestoes([
            new Questao("Quanto é 2 + 2?", materiaQuatroOperacoes),
            new Questao("Quanto é 53 + 38?", materiaQuatroOperacoes),
            new Questao("Quanto é 985 + 15?", materiaQuatroOperacoes),
            new Questao("Quanto é 9 / 3?", materiaQuatroOperacoes),
            new Questao("Quanto é 30 * 15?", materiaQuatroOperacoes)
        ]);

        materiaFracoes.AdicionarQuestoes([
            new Questao("Qual é a fração que representa a metade de uma pizza?", materiaFracoes),
            new Questao("Qual fração representa três partes de um total de quatro partes iguais?", materiaFracoes),
            new Questao("Qual é o resultado da soma: 1/4 + 1/4?", materiaFracoes),
            new Questao("Qual é a fração equivalente a 2/4?", materiaFracoes),
            new Questao("Se você tem uma barra de chocolate dividida em 8 pedaços e come 3, qual fração representa o que você comeu?", materiaFracoes)
        ]);

        disciplina.AdicionarMateria(materiaQuatroOperacoes);
        disciplina.AdicionarMateria(materiaFracoes);

        // Act
        var questoesSorteadas = disciplina.ObterQuestoesAleatorias(10, SerieMateria.SegundaSerie);

        // Assert
        List<Questao> questoesEsperadas = [.. materiaQuatroOperacoes.Questoes, .. materiaFracoes.Questoes];

        Assert.AreEqual(10, questoesSorteadas.Count);
        CollectionAssert.IsSubsetOf(questoesSorteadas, questoesEsperadas);
    }
}