using FizzWare.NBuilder;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Dominio.ModuloQuestao;
using TesteFacil.Dominio.ModuloTeste;
using TesteFacil.Testes.Integracao.Compartilhado;

namespace TesteFacil.Testes.Integracao.ModuloTeste;

[TestClass]
[TestCategory("Testes de Integração de Teste")]
public sealed class RepositorioTesteEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Teste_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew()
            .With(d => d.Nome = "Matemática")
            .Persist();

        var materia = Builder<Materia>.CreateNew()
            .With(m => m.Nome = "Quatro Operações")
            .With(m => m.Disciplina = disciplina)
            .Persist();

        var questoes = Builder<Questao>.CreateListOfSize(5)
            .All()
            .With(q => q.Materia = materia)
            .Persist()
            .ToList();

        var teste = new Teste(
            titulo: "Teste de Matemática",
            recuperacao: false,
            quantidadeQuestoes: 5,
            serieMateria: materia.Serie,
            disciplina,
            materia,
            questoes
        );

        // Act
        repositorioTeste?.Cadastrar(teste);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioTeste?.SelecionarRegistroPorId(teste.Id);

        Assert.AreEqual(teste, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Teste_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew()
            .With(d => d.Nome = "Matemática")
            .Persist();

        var materia = Builder<Materia>.CreateNew()
            .With(m => m.Nome = "Quatro Operações")
            .With(m => m.Disciplina = disciplina)
            .Persist();

        var questoes = Builder<Questao>.CreateListOfSize(5)
            .All()
            .With(q => q.Materia = materia)
            .Persist()
            .ToList();

        var teste = new Teste(
            titulo: "Teste de Matemática",
            recuperacao: false,
            quantidadeQuestoes: 5,
            serieMateria: materia.Serie,
            disciplina,
            materia,
            questoes
        );

        repositorioTeste?.Cadastrar(teste);
        dbContext?.SaveChanges();

        // Act
        var conseguiuExcluir = repositorioTeste?.Excluir(teste.Id);
        dbContext?.SaveChanges();


        // Assert
        var registroSelecionado = repositorioTeste?.SelecionarRegistroPorId(teste.Id);

        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Testes_Corretamente()
    {
        // Arrange
        var disciplina = Builder<Disciplina>.CreateNew()
           .With(d => d.Nome = "Matemática")
           .Persist();

        var materia = Builder<Materia>.CreateNew()
            .With(m => m.Nome = "Quatro Operações")
            .With(m => m.Disciplina = disciplina)
            .Persist();

        var questoes = Builder<Questao>.CreateListOfSize(5)
            .All()
            .With(q => q.Materia = materia)
            .Persist()
            .ToList();

        var teste = new Teste(
            titulo: "Teste de Matemática",
            recuperacao: false,
            quantidadeQuestoes: 5,
            serieMateria: materia.Serie,
            disciplina,
            materia,
            questoes
        );

        var teste2 = new Teste(
            titulo: "Teste de Matemática de Recuperação",
            recuperacao: true,
            quantidadeQuestoes: 5,
            serieMateria: materia.Serie,
            disciplina,
            materia: null
        );

        List<Teste> registrosEsperados = [teste, teste2];

        repositorioTeste?.CadastrarEntidades(registrosEsperados);
        dbContext?.SaveChanges();

        // Act
        var registrosRecebidos = repositorioTeste?.SelecionarRegistros();

        // Assert
        var registrosEsperadosOrdenados = registrosEsperados
            .OrderBy(x => x.Titulo).ToList();

        CollectionAssert.AreEqual(registrosEsperadosOrdenados, registrosRecebidos);
    }
}