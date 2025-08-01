using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TesteFacil.Dominio.ModuloDisciplina;

namespace TesteFacil.Infraestrutura.Orm.ModuloDisciplina;

public class MapeadorDisciplinaEmOrm : IEntityTypeConfiguration<Disciplina>
{
    public void Configure(EntityTypeBuilder<Disciplina> builder)
    {
        builder.Property(d => d.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(d => d.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasMany(d => d.Materias)
            .WithOne(m => m.Disciplina)
            .OnDelete(DeleteBehavior.NoAction);
    }
}