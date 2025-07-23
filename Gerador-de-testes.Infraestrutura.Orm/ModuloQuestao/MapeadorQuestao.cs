using Gerador_de_testes.ModuloDisciplina;
using Gerador_de_testes.ModuloQuestao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerador_de_testes.Infraestrutura.Orm.ModuloGestao;

public class MapeadorQuestao : IEntityTypeConfiguration<Questao>
{
    public void Configure(EntityTypeBuilder<Questao> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Enunciado)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasOne(q => q.Materia)
            .WithMany(m => m.Questoes);
            
        builder.HasMany(q => q.Alternativas)
            .WithOne(a => a.Questao);
    }
}