using Microsoft.EntityFrameworkCore;
using TesteFacil.Dominio.Compartilhado;

namespace TesteFacil.Infraestrutura.Orm.Compartilhado;

public class RepositorioBaseEmOrm<T> where T : EntidadeBase<T>
{
    protected readonly DbSet<T> registros;

    public RepositorioBaseEmOrm(TesteFacilDbContext contexto)
    {
        this.registros = contexto.Set<T>();
    }

    public void Cadastrar(T novoRegistro)
    {
        registros.Add(novoRegistro);
    }
    public void CadastrarEntidades(IList<T> entidades)
    {
        registros.AddRange(entidades);
    }

    public bool Editar(Guid idRegistro, T registroEditado)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registroSelecionado.AtualizarRegistro(registroEditado);

        return true;
    }

    public bool Excluir(Guid idRegistro)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registros.Remove(registroSelecionado);

        return true;
    }

    public virtual T? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros.FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public virtual List<T> SelecionarRegistros()
    {
        return registros.ToList();
    }
}