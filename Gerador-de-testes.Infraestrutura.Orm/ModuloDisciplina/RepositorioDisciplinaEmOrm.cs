using Microsoft.EntityFrameworkCore;
using TesteFacil.Dominio.ModuloDisciplina;
using TesteFacil.Infraestrutura.Orm.Compartilhado;

namespace TesteFacil.Infraestrutura.Orm.ModuloDisciplina;

public class RepositorioDisciplinaEmOrm : RepositorioBaseEmOrm<Disciplina>, IRepositorioDisciplina
{
    public RepositorioDisciplinaEmOrm(TesteFacilDbContext contexto) : base(contexto)
    {
    }

    public override Disciplina? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros
            .Include(x => x.Materias)
            .FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public override List<Disciplina> SelecionarRegistros()
    {
        return registros
            .Include(x => x.Materias)
            .OrderBy(x => x.Nome)
            .ToList();
    }
}