namespace TesteFacil.Testes.Integracao.Compartilhado;

[TestClass]
public static class AssemblyConfig
{
    public static TesteDbContextFactory? DbContextFactory { get; set; }

    [AssemblyInitialize]
    public static async Task Setup(TestContext _)
    {
        DbContextFactory = new TesteDbContextFactory();

        await DbContextFactory.InicializarAsync();
    }

    [AssemblyCleanup]
    public static async Task Cleanup()
    {
        if (DbContextFactory != null)
            await DbContextFactory.EncerrarAsync();
    }
}