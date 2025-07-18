using Gerador_de_testes.ModuloMateria;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Gerador_de_testes.Infraestrutura.Orm.ModuloMateria;

public class MapeadorMateria : IEntityTypeConfiguration<Materia>
{
    public void Configure(EntityTypeBuilder<Materia> builder)
    {
        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Serie)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(x => x.Disciplina)
            .WithMany(d => d.Materias);

        builder.HasMany(m => m.Questoes)
                .WithOne(q => q.Materia);
    }
}