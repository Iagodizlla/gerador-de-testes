using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloMateria;
using Microsoft.EntityFrameworkCore;

namespace Gerador_de_testes.Infraestrutura.Orm.ModuloMateria;

public class RepositorioMateriaEmOrm : RepositorioBaseEmOrm<Materia>, IRepositorioMateria
{
    private readonly DbSet<Materia> registros;
    public RepositorioMateriaEmOrm(GeradorDeTestesDbContext contextoDados) : base(contextoDados)
    {
        this.registros = contextoDados.Set<Materia>();
    }

    public void CadastrarRegistro(Materia novoRegistro)
    {
        registros.Add(novoRegistro);
    }

    public bool EditarRegistro(Guid idRegistro, Materia registroEditado)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registroSelecionado.AtualizarRegistro(registroEditado);

        return true;
    }

    public bool ExcluirRegistro(Guid idRegistro)
    {
        var registroSelecionado = SelecionarRegistroPorId(idRegistro);

        if (registroSelecionado is null)
            return false;

        registros.Remove(registroSelecionado);

        return true;
    }

    public virtual Materia? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros.FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public virtual List<Materia> SelecionarRegistros()
    {
        return registros.Include(x => x.Disciplina).ToList();
    }
}