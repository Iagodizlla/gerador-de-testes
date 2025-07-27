using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TesteFacil.Dominio.ModuloQuestao;

namespace TesteFacil.Infraestrutura.Orm.ModuloQuestao;

public class MapeadorAlternativaEmOrm : IEntityTypeConfiguration<Alternativa>
{
    public void Configure(EntityTypeBuilder<Alternativa> builder)
    {
        builder.Property(a => a.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(a => a.Letra)
            .IsRequired();

        builder.Property(a => a.Resposta)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(a => a.Correta)
            .IsRequired();

        builder.HasOne(a => a.Questao)
            .WithMany(q => q.Alternativas)
            .IsRequired();
    }
}