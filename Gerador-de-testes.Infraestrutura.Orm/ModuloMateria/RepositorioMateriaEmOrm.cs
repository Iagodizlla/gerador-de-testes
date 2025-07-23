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

    public override List<Materia> SelecionarRegistros()
    {
        return registros.Include(x => x.Disciplina).ToList();
    }
}