using FizzWare.NBuilder;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Infraestrutura.Orm.Compartilhado;
using TesteFacil.Infraestrutura.Orm.ModuloDisciplina;
using TesteFacil.Infraestrutura.Orm.ModuloMateria;
using TesteFacil.Infraestrutura.Orm.ModuloQuestao;
using TesteFacil.Infraestrutura.Orm.ModuloTeste;

namespace TesteFacil.Testes.Integracao.Compartilhado;

[TestClass]
public class TestFixture
{
    private static TesteFacilDbContextFactory? factory;
    protected TesteFacilDbContext? dbContext;
    protected RepositorioTesteEmOrm? repositorioTeste;
    protected RepositorioQuestaoEmOrm? repositorioQuestao;
    protected RepositorioMateriaEmOrm? repositorioMateria;
    protected RepositorioDisciplinaEmOrm? repositorioDisciplina;


    [AssemblyInitialize]
    public static async Task Setup(TestContext _)
    {
        factory = new TesteFacilDbContextFactory();

        await factory.InicializarAsync();
    }

    [AssemblyCleanup]
    public static async Task Teardown()
    {
        if (factory is not null)
            await factory.EncerrarAsync();
    }

    [TestInitialize]
    public void ConfigurarTestes()
    {
        dbContext = factory?.CriarDbContext();

        if (dbContext is null)
            throw new ArgumentNullException("DbContextFactory não inicializada");

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
}