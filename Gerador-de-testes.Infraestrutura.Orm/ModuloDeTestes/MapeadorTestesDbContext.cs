using Gerador_de_testes.ModuloDeTestes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gerador_de_testes.Infraestrutura.Orm.ModuloDeTestes
{
    public class MapeadorTestesDbContext : IEntityTypeConfiguration<Teste>
    {
        public void Configure(EntityTypeBuilder<Teste> builder)
        {

            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(x => x.Titulo)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Serie)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(x => x.Disciplina)
                .WithMany()
                .HasForeignKey("DisciplinaId")
                .IsRequired();

            // Aguardando a implementação de Materias
            // builder.HasMany(x => x.Materias)
            //     .WithMany()
            //     .hasForeignKey("MateriasId")
            //     .IsRequired();

            builder.Property(x => x.QteQuestoes)
                .HasDefaultValue(0)
                .IsRequired();


        }
    }
}
