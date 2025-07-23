using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloMateria;
using Microsoft.EntityFrameworkCore;
namespace Gerador_de_testes.Infraestrutura.Orm.ModuloDisciplina;

public class RepositorioDisciplinaEmOrm : RepositorioBaseEmOrm<Disciplina>, IRepositorioDisciplina
{
    private readonly DbSet<Disciplina> registros;
    public RepositorioDisciplinaEmOrm(GeradorDeTestesDbContext contextoDados) : base(contextoDados)
    {
        this.registros = contextoDados.Set<Disciplina>();
    }

    public override Disciplina SelecionarRegistroPorId(Guid id)
    {
        return registros.Include(d => d.Materias).FirstOrDefault(d => d.Id == id);
    }
}