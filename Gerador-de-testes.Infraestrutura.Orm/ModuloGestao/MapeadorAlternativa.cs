using Gerador_de_testes.ModuloQuestao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gerador_de_testes.Infraestrutura.Orm.ModuloGestao
{
    public class MapeadorAlternativa : IEntityTypeConfiguration<Alternativa>
    {
        public void Configure(EntityTypeBuilder<Alternativa> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.Resposta)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Correta);
         
            builder.HasOne(a => a.Questao)
                .WithMany(q => q.Alternativas);
        }
    }
}
