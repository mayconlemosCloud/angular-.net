namespace Domain.Entities
{
    public class LivroAutor
    {
        public int LivroCod { get; set; }
        public Livro Livro { get; set; }

        public int AutorCodAu { get; set; }
        public Autor Autor { get; set; }
    }
}
