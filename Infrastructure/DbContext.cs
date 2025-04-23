using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Livro> Livros { get; set; }
        public DbSet<Autor> Autores { get; set; }
        public DbSet<Assunto> Assuntos { get; set; }
        public DbSet<LivroAutor> LivroAutores { get; set; }
        public DbSet<LivroAssunto> LivroAssuntos { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(e => e.Cod);
                entity.Property(e => e.Titulo)
                    .HasMaxLength(40)
                    .IsRequired();

                entity.Property(e => e.Editora)
                    .HasMaxLength(40);

                entity.Property(e => e.AnoPublicacao)
                    .HasMaxLength(4);
            });

       
            modelBuilder.Entity<Autor>(entity =>
            {
                entity.HasKey(e => e.CodAu);
                entity.Property(e => e.Nome)
                    .HasMaxLength(40)
                    .IsRequired();
            });

   
            modelBuilder.Entity<Assunto>(entity =>
            {
                entity.HasKey(e => e.CodAs);
                entity.Property(e => e.Descricao)
                    .HasMaxLength(20)
                    .IsRequired();
            });

       
            modelBuilder.Entity<LivroAutor>()
                .HasKey(la => new { la.LivroCod, la.AutorCodAu });

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.LivroAutores)
                .HasForeignKey(la => la.LivroCod);

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.LivroAutores)
                .HasForeignKey(la => la.AutorCodAu);

      
            modelBuilder.Entity<LivroAssunto>()
                .HasKey(la => new { la.LivroCod, la.AssuntoCodAs });

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.LivroAssuntos)
                .HasForeignKey(la => la.LivroCod);

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Assunto)
                .WithMany(a => a.LivroAssuntos)
                .HasForeignKey(la => la.AssuntoCodAs);
        }
    }
}