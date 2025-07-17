using Gerador_de_testes.ModuloDisciplina;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerador_de_testes.Infraestrutura.Orm.ModuloDisciplina;

public class MapeadorDisciplina : IEntityTypeConfiguration<Disciplina>
{
    public void Configure(EntityTypeBuilder<Disciplina> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(x => x.Materias)
            .WithOne(m => m.Disciplina);
    }
}