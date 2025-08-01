using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TesteFacil.Dominio.ModuloMateria;

namespace TesteFacil.Infraestrutura.Orm.ModuloMateria;

public class MapeadorMateriaEmOrm : IEntityTypeConfiguration<Materia>
{
    public void Configure(EntityTypeBuilder<Materia> builder)
    {
        builder.Property(m => m.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(m => m.Nome)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.Serie)
            .IsRequired()
            .HasConversion<int>();

        builder.HasOne(m => m.Disciplina)
            .WithMany(d => d.Materias)
            .OnDelete(DeleteBehavior.NoAction)
            .IsRequired();
    }
}