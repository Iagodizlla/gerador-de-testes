using DotNet.Testcontainers.Containers;
using FizzWare.NBuilder;
using Testcontainers.MsSql;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Infraestrutura.Orm.Compartilhado;
using TesteFacil.Infraestrutura.Orm.ModuloDisciplina;
using TesteFacil.Infraestrutura.Orm.ModuloMateria;
using TesteFacil.Infraestrutura.Orm.ModuloQuestao;
using TesteFacil.Infraestrutura.Orm.ModuloTeste;

namespace TesteFacil.Testes.Integracao.Compartilhado;

[TestClass]
public abstract class TestFixture
{
    protected TesteFacilDbContext? dbContext;
    protected RepositorioTesteEmOrm? repositorioTeste;
    protected RepositorioQuestaoEmOrm? repositorioQuestao;
    protected RepositorioMateriaEmOrm? repositorioMateria;
    protected RepositorioDisciplinaEmOrm? repositorioDisciplina;
    private static IDatabaseContainer? container;

    [AssemblyInitialize]
    public static async Task Setup(TestContext _)
    {
        container = new MsSqlBuilder()
           .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
           .WithName("teste-facil-testdb-container")
           .WithCleanUp(true)
           .Build();

        await InicializarBancoDadosAsync(container);
    }

    [AssemblyCleanup]
    public static async Task Teardown()
    {
        await EncerrarBancoDadosAsync();
    }

    [TestInitialize]
    public void ConfigurarTestes()
    {
        if (container is null)
            throw new ArgumentNullException("O banco de dados não foi inicializado.");

        dbContext = TesteFacilDbContextFactory.CriarDbContext(container.GetConnectionString());

        ConfigurarTabelas(dbContext);

        repositorioTeste = new RepositorioTesteEmOrm(dbContext);
        repositorioQuestao = new RepositorioQuestaoEmOrm(dbContext);
        repositorioDisciplina = new RepositorioDisciplinaEmOrm(dbContext);
        repositorioMateria = new RepositorioMateriaEmOrm(dbContext);

        BuilderSetup.SetCreatePersistenceMethod<Disciplina>(repositorioDisciplina.Cadastrar);
        BuilderSetup.SetCreatePersistenceMethod<IList<Disciplina>>(repositorioDisciplina.CadastrarEntidades);
    }

    private static void ConfigurarTabelas(TesteFacilDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.Testes.RemoveRange(dbContext.Testes);
        dbContext.Questoes.RemoveRange(dbContext.Questoes);
        dbContext.Materias.RemoveRange(dbContext.Materias);
        dbContext.Disciplinas.RemoveRange(dbContext.Disciplinas);

        dbContext.SaveChanges();
    }
    private static async Task InicializarBancoDadosAsync(IDatabaseContainer container)
    {
        await container.StartAsync();
    }

    private static async Task EncerrarBancoDadosAsync()
    {
        if (container is null)
            throw new ArgumentNullException("O Banco de dados não foi inicializado.");

        await container.StopAsync();
        await container.DisposeAsync();
    }
}