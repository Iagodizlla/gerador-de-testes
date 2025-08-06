using FizzWare.NBuilder;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Testes.Integracao.Compartilhado;

namespace TesteFacil.Testes.Integracao.ModuloQuestao;

[TestClass]
[TestCategory("Testes de Integração de Questão")]
public sealed class RepositorioQuestaoEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Questao_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew()
            .With(d => d.Nome = "Matemática")
            .Persist();

        var materia = Builder<Materia>.CreateNew()
            .With(m => m.Nome = "Quatro Operações")
            .With(m => m.Disciplina = disciplina)
            .Persist();

        var questao = new Questao("Quanto é 2 + 2?", materia);

        // Act
        repositorioQuestao?.Cadastrar(questao);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioQuestao?.SelecionarRegistroPorId(questao.Id);

        Assert.AreEqual(questao, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Editar_Questao_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew()
            .With(d => d.Nome = "Matemática")
            .Persist();

        var materia = Builder<Materia>.CreateNew()
            .With(m => m.Nome = "Quatro Operações")
            .With(m => m.Disciplina = disciplina)
            .Persist();

        var questao = new Questao("Quanto é 2 + 2?", materia);

        repositorioQuestao?.Cadastrar(questao);
        dbContext?.SaveChanges();

        var questaoEditada = new Questao("Quanto é 5 + 9?", materia);

        // Act
        var conseguiuEditar = repositorioQuestao?.Editar(questao.Id, questaoEditada);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioQuestao?.SelecionarRegistroPorId(questao.Id);

        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(questao, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Questao_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew()
            .With(d => d.Nome = "Matemática")
            .Persist();

        var materia = Builder<Materia>.CreateNew()
            .With(m => m.Nome = "Quatro Operações")
            .With(m => m.Disciplina = disciplina)
            .Persist();

        var questao = new Questao("Quanto é 2 + 2?", materia);

        repositorioQuestao?.Cadastrar(questao);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = repositorioQuestao?.Excluir(questao.Id);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioQuestao?.SelecionarRegistroPorId(questao.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Questoes_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew()
            .With(d => d.Nome = "Matemática")
            .Persist();

        var materias = Builder<Materia>.CreateListOfSize(3)
            .All()
            .With(m => m.Disciplina = disciplina)
            .Persist();

        var questao = new Questao("Quanto é 2 + 2?", materias[0]);
        var questao2 = new Questao("Quanto é 85 - 42?", materias[1]);
        var questao3 = new Questao("Quanto é 15 / 5?", materias[2]);

        List<Questao> registrosEsperados = [questao, questao2, questao3];

        repositorioQuestao?.CadastrarEntidades(registrosEsperados);
        dbContext?.SaveChanges();

        // Act
        var registrosRecebidos = repositorioQuestao?.SelecionarRegistros();

        // Assert
        var registrosEsperadosOrdenados = registrosEsperados
            .OrderBy(x => x.Enunciado).ToList();

        CollectionAssert.AreEqual(registrosEsperadosOrdenados, registrosRecebidos);
    }
}