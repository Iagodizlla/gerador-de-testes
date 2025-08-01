using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Testes.Integracao.Compartilhado;

namespace TesteFacil.Testes.Integracao.ModuloDisciplina;

[TestClass]
[TestCategory("Testes de Integração de Disciplina")]
public sealed class RepositorioDisciplinaEmOrmTests : TestFixture
{
    

    [TestMethod]
    public void Deve_Cadastrar_Disciplina_Corretamente()
    {
        // Arrange
        var disciplina = new Disciplina("Matemática");

        // Act
        repositorioDisciplina?.Cadastrar(disciplina);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioDisciplina?.SelecionarRegistroPorId(disciplina.Id);

        Assert.AreEqual(disciplina, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Editar_Disciplina_Corretamente()
    {
        // Arrange
        var disciplina = new Disciplina("Matemática");
        repositorioDisciplina?.Cadastrar(disciplina);
        dbContext?.SaveChanges();

        var disciplinaEditada = new Disciplina("Física");

        // Act
        var conseguiuEditar = repositorioDisciplina?.Editar(disciplina.Id, disciplinaEditada);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioDisciplina?.SelecionarRegistroPorId(disciplina.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(disciplina, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Disciplina_Corretamente()
    {
        // Arrange
        var disciplina = new Disciplina("Matemática");
        repositorioDisciplina?.Cadastrar(disciplina);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = repositorioDisciplina?.Excluir(disciplina.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioDisciplina?.SelecionarRegistroPorId(disciplina.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Disciplinas_Corretamente()
    {
        // Arrange - Arranjo
        var disciplina = new Disciplina("Matemática");
        var disciplina2 = new Disciplina("Ciências");
        var disciplina3 = new Disciplina("Geografia");

        List<Disciplina> disciplinasEsperadas = [disciplina, disciplina2, disciplina3];

        repositorioDisciplina?.CadastrarEntidades(disciplinasEsperadas);
        dbContext?.SaveChanges();

        var disciplinasEsperadasOrdenadas = disciplinasEsperadas
            .OrderBy(d => d.Nome)
            .ToList();

        // Act - Ação
        var disciplinasRecebidas = repositorioDisciplina?.SelecionarRegistros();

        // Assert - Asseção
        CollectionAssert.AreEqual(disciplinasEsperadasOrdenadas, disciplinasRecebidas);
    }
}