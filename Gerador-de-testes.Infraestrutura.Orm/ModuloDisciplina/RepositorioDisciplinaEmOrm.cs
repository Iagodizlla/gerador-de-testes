using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using Microsoft.EntityFrameworkCore;
namespace Gerador_de_testes.Infraestrutura.Orm.ModuloDisciplina;

public class RepositorioDisciplinaEmOrm : RepositorioBaseEmOrm<Disciplina>, IRepositorioDisciplina
{
    public RepositorioDisciplinaEmOrm(GeradorDeTestesDbContext contexto) : base(contexto)
    {
    }
}