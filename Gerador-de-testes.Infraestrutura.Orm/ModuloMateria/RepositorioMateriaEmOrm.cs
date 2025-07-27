using Microsoft.EntityFrameworkCore;
using TesteFacil.Dominio.ModuloMateria;
using TesteFacil.Infraestrutura.Orm.Compartilhado;

namespace TesteFacil.Infraestrutura.Orm.ModuloMateria;

public class RepositorioMateriaEmOrm : RepositorioBaseEmOrm<Materia>, IRepositorioMateria
{
    public RepositorioMateriaEmOrm(TesteFacilDbContext contexto) : base(contexto)
    {
    }

    public override Materia? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros
            .Include(x => x.Disciplina)
            .FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public override List<Materia> SelecionarRegistros()
    {
        return registros
            .Include(x => x.Disciplina)
            .ToList();
    }
}