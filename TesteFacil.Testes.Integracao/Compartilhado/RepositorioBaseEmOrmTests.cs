using FizzWare.NBuilder;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Infraestrutura.Orm.Compartilhado;
using TesteFacil.Infraestrutura.Orm.ModuloDisciplina;
using TesteFacil.Infraestrutura.Orm.ModuloMateria;

namespace TesteFacil.Testes.Integracao.Compartilhado;

[TestClass]
public abstract class RepositorioBaseEmOrmTests
{
    private static TesteFacilDbContextFactory? factory;
    protected TesteFacilDbContext? dbContext;
    protected RepositorioDisciplinaEmOrm? repositorioDisciplina;
    protected RepositorioMateriaEmOrm? repositorioMateria;

    [AssemblyInitialize]
    public static async Task Setup(TestContext _)
    {
        factory = new TesteFacilDbContextFactory();

        await factory.InicializarAsync();
    }

    [AssemblyCleanup]
    public static async Task Cleanup()
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

        repositorioDisciplina = new RepositorioDisciplinaEmOrm(dbContext);
        repositorioMateria = new RepositorioMateriaEmOrm(dbContext);

        BuilderSetup.SetCreatePersistenceMethod<Disciplina>(repositorioDisciplina.Cadastrar);
        BuilderSetup.SetCreatePersistenceMethod<IList<Disciplina>>(CadastrarDisciplinas);
    }

    private void CadastrarDisciplinas(IList<Disciplina> disciplinas)
    {
        dbContext?.Disciplinas.AddRange(disciplinas);

        dbContext?.SaveChanges();
    }
}