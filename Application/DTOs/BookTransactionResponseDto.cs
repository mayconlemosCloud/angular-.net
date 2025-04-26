using System;

namespace Application.Dtos
{
    public class BookTransactionResponseDto
    {
        public int Codtr { get; set; }
        public int LivroCodl { get; set; }
        public DateTime CriadoEm { get; set; }
        public string FormaDeCompra { get; set; }
    }
}
