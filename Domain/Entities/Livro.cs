using System.Collections.Generic;

namespace Domain.Entities
{
    public class Livro
    {
        public int Codl { get; set; }
        public string Titulo { get; set; }
        public string Editora { get; set; }
        public int Edicao { get; set; }
        public string AnoPublicacao { get; set; }
        public decimal Preco { get; set; }

        public ICollection<LivroAutor> LivroAutores { get; set; }
        public ICollection<LivroAssunto> LivroAssuntos { get; set; }
    }
}
