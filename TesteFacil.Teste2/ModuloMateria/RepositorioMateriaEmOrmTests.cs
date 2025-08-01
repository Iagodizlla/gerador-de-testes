using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Testes.Integracao.Compartilhado;
using FizzWare.NBuilder;

namespace TesteFacil.Testes.Integracao.ModuloMateria;

[TestClass]
[TestCategory("Testes de Integração de Matéria")]
public class RepositorioMateriaEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Materia_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew().Persist();

        var materia = new Materia("Quatro Operações", SerieMateria.SegundaSerie, disciplina);

        // Act
        repositorioMateria?.Cadastrar(materia);
        dbContext?.SaveChanges();

        // Assert
        var materiaSelecionada = repositorioMateria?.SelecionarRegistroPorId(materia.Id);

        Assert.AreEqual(materia, materiaSelecionada);
    }

    [TestMethod]
    public void Deve_Editar_Materia_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew().Persist();

        var materia = new Materia("Quatro Operações", SerieMateria.SegundaSerie, disciplina);
        repositorioMateria?.Cadastrar(materia);
        dbContext?.SaveChanges();

        var materiaEditada = new Materia("Álgebra Linear", SerieMateria.SegundaSerie, disciplina);

        // Act
        var conseguiuEditar = repositorioMateria?.Editar(materia.Id, materiaEditada);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioMateria?.SelecionarRegistroPorId(materia.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(materia, registroSelecionado);
    }


    [TestMethod]
    public void Deve_Excluir_Materia_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew().Persist();

        var materia = new Materia("Quatro Operações", SerieMateria.SegundaSerie, disciplina);
        repositorioMateria?.Cadastrar(materia);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = repositorioMateria?.Excluir(materia.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioMateria?.SelecionarRegistroPorId(materia.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Materias_Corretamente()
    {
        // Arrange - Arranjo
        var disciplinas = Builder<Disciplina>.CreateListOfSize(3).Persist().ToList();

        var materia = new Materia("Quatro Operações", SerieMateria.PrimeiraSerie, disciplinas[0]);
        var materia2 = new Materia("Álgebra Linear", SerieMateria.PrimeiraSerie, disciplinas[1]);
        var materia3 = new Materia("Cálculo Numérico", SerieMateria.PrimeiraSerie, disciplinas[2]);

        List<Materia> materiasEsperadas = [materia, materia2, materia3];

        repositorioMateria?.CadastrarEntidades(materiasEsperadas);
        dbContext?.SaveChanges();

        var materiasEsperadasOrdenadas = materiasEsperadas
            .OrderBy(d => d.Nome)
            .ToList();

        // Act - Ação
        var materiasRecebidas = repositorioMateria?.SelecionarRegistros();

        // Assert - Asseção
        CollectionAssert.AreEqual(materiasEsperadasOrdenadas, materiasRecebidas);
    }
}