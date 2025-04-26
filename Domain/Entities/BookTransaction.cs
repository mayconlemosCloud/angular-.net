using System;

namespace Domain.Entities
{
    public class BookTransaction
    {
        public int Codtr { get; set; }
        public int LivroCodl { get; set; } 
        public DateTime CriadoEm { get; set; }
        public Livro Livro { get; set; }
        public string FormaDeCompra { get; set; }
    }
}
