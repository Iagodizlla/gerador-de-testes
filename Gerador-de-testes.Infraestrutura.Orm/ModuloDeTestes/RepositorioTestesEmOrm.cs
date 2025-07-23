using Gerador_de_testes.Compartilhado;
using Gerador_de_testes.Infraestrutura.Orm.Compartilhado;
using Gerador_de_testes.ModuloDeTestes;
using Gerador_de_testes.ModuloDisciplina;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_de_testes.Infraestrutura.Orm.ModuloDeTestes;

public class RepositorioTestesEmOrm : RepositorioBaseEmOrm<Teste>, IRepositorioTeste
{
    private readonly DbSet<Teste> registros;
    public RepositorioTestesEmOrm(GeradorDeTestesDbContext contextoDados) : base(contextoDados)
    {
        this.registros = contextoDados.Set<Teste>();
    }

    public List<Teste> SelecionarTestesPorDisciplina(Guid idDisciplina)
    {
        throw new NotImplementedException();
    }

    public List<Teste> SelecionarTestesPorQuantidadeQuestoes(int quantidadeQuestoes)
    {
        throw new NotImplementedException();
    }

    public List<Teste> SelecionarTestesPorSerie(string serie)
    {
        throw new NotImplementedException();
    }

    public List<Teste> SelecionarTestesPorTitulo(string titulo)
    {
        throw new NotImplementedException();
    }
    public override Teste? SelecionarRegistroPorId(Guid idRegistro)
    {
        return registros.Include(t => t.Disciplinas).Include(t => t.Materias).Include(t => t.QuestoesSelecionadas).FirstOrDefault(x => x.Id.Equals(idRegistro));

    }
}