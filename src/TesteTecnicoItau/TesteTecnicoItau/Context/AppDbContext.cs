using Microsoft.EntityFrameworkCore;
using TesteTecnicoItau.Entities;

namespace TesteTecnicoItau.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Ativo> Ativos { get; set; } = null!;
        public DbSet<Operacao> Operacoes { get; set; } = null!;
        public DbSet<Cotacao> Cotacoes { get; set; } = null!;
        public DbSet<Posicao> Posicoes { get; set; } = null!;

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
            });

            modelBuilder.Entity<Cotacao>(entity =>
            {
                entity.ToTable("cotacoes");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.AtivoId).HasColumnName("ativo_id");
                entity.Property(c => c.PrecoUnit).HasColumnName("preco_unit");
                entity.Property(c => c.DataHora).HasColumnName("data_hora");
            });
        }
    }
}