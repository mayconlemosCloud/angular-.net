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

        public DbSet<BookTransaction> BookTransactions { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<Livro>(entity =>
            {
                entity.HasKey(e => e.Codl);
                entity.Property(e => e.Codl)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Titulo)
                    .HasMaxLength(40)
                    .IsRequired();

                entity.Property(e => e.Editora)
                    .HasMaxLength(40);

                entity.Property(e => e.AnoPublicacao)
                    .HasMaxLength(4);

                entity.Property(e => e.Preco)
                    .HasPrecision(10, 2)
                    .HasDefaultValue(0.00m);   
            });

       
            modelBuilder.Entity<Autor>(entity =>
            {
                entity.HasKey(e => e.CodAu);
                entity.Property(e => e.CodAu)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Nome)
                    .HasMaxLength(40)
                    .IsRequired();
            });

   
            modelBuilder.Entity<Assunto>(entity =>
            {
                entity.HasKey(e => e.CodAs);
                entity.Property(e => e.CodAs)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.Descricao)
                    .HasMaxLength(20)
                    .IsRequired();
            });

       
            modelBuilder.Entity<LivroAutor>()
                .HasKey(la => new { la.LivroCodl, la.AutorCodAu });

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.LivroAutores)
                .HasForeignKey(la => la.LivroCodl);

            modelBuilder.Entity<LivroAutor>()
                .HasOne(la => la.Autor)
                .WithMany(a => a.LivroAutores)
                .HasForeignKey(la => la.AutorCodAu);

      
            modelBuilder.Entity<LivroAssunto>()
                .HasKey(la => new { la.LivroCodl, la.AssuntoCodAs });

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Livro)
                .WithMany(l => l.LivroAssuntos)
                .HasForeignKey(la => la.LivroCodl);

            modelBuilder.Entity<LivroAssunto>()
                .HasOne(la => la.Assunto)
                .WithMany(a => a.LivroAssuntos)
                .HasForeignKey(la => la.AssuntoCodAs);


            modelBuilder.Entity<BookTransaction>(entity =>
            {
                entity.HasKey(e => e.Codtr);
                entity.Property(e => e.Codtr)
                    .ValueGeneratedOnAdd();
                entity.Property(e => e.LivroCodl)
                    .IsRequired();
                entity.Property(e => e.CriadoEm)
                    .IsRequired()
                    .HasConversion(
                        v => v.ToUniversalTime(), 
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc) 
                    );
                    
                entity.Property(e => e.FormaDeCompra)
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasOne(e => e.Livro)
                    .WithMany()
                    .HasForeignKey(e => e.LivroCodl);        
            });    

            modelBuilder.Entity<Livro>().HasData(
                new Livro { Codl = 1, Titulo = "Dom Casmurro", Editora = "Companhia das Letras", Edicao = 1, AnoPublicacao = "1899", Preco = 49.90m },
                new Livro { Codl = 2, Titulo = "O Alquimista", Editora = "HarperCollins", Edicao = 1, AnoPublicacao = "1988", Preco = 39.90m },
                new Livro { Codl = 3, Titulo = "Bíblia Sagrada", Editora = "Sociedade Bíblica do Brasil", Edicao = 1, AnoPublicacao = "1969", Preco = 0.00m },
                new Livro { Codl = 4, Titulo = "Death Note", Editora = "Shueisha", Edicao = 1, AnoPublicacao = "2003", Preco = 59.90m }
            );

            modelBuilder.Entity<Autor>().HasData(
                new Autor { CodAu = 1, Nome = "Machado de Assis" },
                new Autor { CodAu = 2, Nome = "Paulo Coelho" },
                new Autor { CodAu = 3, Nome = "Diversos Autores" },
                new Autor { CodAu = 4, Nome = "Tsugumi Ohba" }
            );

            modelBuilder.Entity<Assunto>().HasData(
                new Assunto { CodAs = 1, Descricao = "Literatura" }, 
                new Assunto { CodAs = 2, Descricao = "Ficção" },
                new Assunto { CodAs = 3, Descricao = "Religião" },
                new Assunto { CodAs = 4, Descricao = "Mangá" }
            );
        }

        public override int SaveChanges()
        {
            ConvertDateTimesToUtc();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ConvertDateTimesToUtc();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ConvertDateTimesToUtc()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    foreach (var property in entry.Properties)
                    {
                        if (property.Metadata.ClrType == typeof(DateTime) && property.CurrentValue is DateTime dateTime)
                        {
                            property.CurrentValue = dateTime.Kind == DateTimeKind.Utc
                                ? dateTime
                                : dateTime.ToUniversalTime();
                        }
                    }
                }
            }
        }
    }
}