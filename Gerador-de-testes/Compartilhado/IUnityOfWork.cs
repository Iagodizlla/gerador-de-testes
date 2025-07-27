namespace TesteFacil.Dominio.Compartilhado;

public interface IUnitOfWork // Padrão Unit of Work
{
    public void Commit();
    public void Rollback();
}