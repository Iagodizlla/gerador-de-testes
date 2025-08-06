using Microsoft.EntityFrameworkCore;
using TesteFacil.Dominio.ModuloTeste;
using TesteFacil.Infraestrutura.Orm.Compartilhado;

namespace TesteFacil.Infraestrutura.Orm.ModuloTeste;

public class RepositorioTesteEmOrm : RepositorioBaseEmOrm<Teste>, IRepositorioTeste
{
    public RepositorioTesteEmOrm(TesteFacilDbContext contexto) : base(contexto)
    {
    }

    public override Teste? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros
            .Include(t => t.Questoes)
            .ThenInclude(q => q.Alternativas)
            .Include(t => t.Questoes)
            .ThenInclude(q => q.Materia)
            .Include(t => t.Disciplina)
            .Include(t => t.Materia)
            .FirstOrDefault(x => x.Id.Equals(idRegistro));
    }

    public override List<Teste> SelecionarRegistros()
    {
        return registros
            .OrderBy(x => x.Titulo)
            .Include(t => t.Questoes)
            .ThenInclude(q => q.Materia)
            .Include(t => t.Disciplina)
            .Include(t => t.Materia)
            .ToList();
    }
}