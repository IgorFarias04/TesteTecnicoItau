using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TesteTecnicoItau.Entities;

namespace TesteTecnicoItau.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Ativo> Ativos { get; set; } = null!;
        public DbSet<Operacao> Operacoes { get; set; } = null!;
        public DbSet<Cotacao> Cotacoes { get; set; } = null!;
        public DbSet<Posicao> Posicoes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;database=sistema_ativos;user=root;password=@Rodrigues2012",
                new MySqlServerVersion(new Version(8, 0, 36))
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operacao>().Property(o => o.TipoOp).HasConversion<string>();

            modelBuilder.Entity<Operacao>().Property(o => o.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Operacao>().Property(o => o.AtivoId).HasColumnName("ativo_id");
            modelBuilder.Entity<Operacao>().Property(o => o.DataHora).HasColumnName("data_hora");
            modelBuilder.Entity<Operacao>().Property(o => o.PrecoUnit).HasColumnName("preco_unit");
            modelBuilder.Entity<Operacao>().Property(o => o.TipoOp).HasColumnName("tipo_op");

            modelBuilder.Entity<Usuario>().Property(u => u.PercCorretagem).HasColumnName("perc_corretagem");

            modelBuilder.Entity<Posicao>(entity =>
            {
                entity.ToTable("posicoes");

                entity.HasKey(p => p.Id);

                entity.Property(p => p.UsuarioId).HasColumnName("usuario_id");
                entity.Property(p => p.AtivoId).HasColumnName("ativo_id");
                entity.Property(p => p.Qtd).HasColumnName("qtd");
                entity.Property(p => p.PrecoMedio).HasColumnName("preco_medio");
                entity.Property(p => p.Pl).HasColumnName("pl");

                //entity.HasOne(p => p.Usuario)
                //      .WithMany()
                //      .HasForeignKey(p => p.UsuarioId)
                //      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(p => p.UsuarioId).HasColumnName("usuario_id");
            });


            // Mapeamento da entidade COTACAO
            modelBuilder.Entity<Cotacao>(entity =>
            {
                entity.ToTable("cotacoes");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.AtivoId).HasColumnName("ativo_id");
                entity.Property(c => c.PrecoUnit).HasColumnName("preco_unit");
                entity.Property(c => c.DataHora).HasColumnName("data_hora");

                entity.Property(p => p.AtivoId).HasColumnName("ativo_id");
            });




        }
    }
}
