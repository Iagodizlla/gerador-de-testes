using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDisciplina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_de_testes.Infraestrutura.Orm.ModuloDisciplina;

public class RepositorioDisciplinaEmOrm : RepositorioBaseEmOrm<Disciplina>, IRepositorioDisciplina
{
    public RepositorioDisciplinaEmOrm(GeradorDeTestesDbContext contexto) : base(contexto)
    {
    }
}