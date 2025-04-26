using System;

namespace Application.Dtos
{
    public class BookTransactionRequestDto
    {
        public int LivroCodl { get; set; }
        public DateTime CriadoEm { get; set; }
        public string FormaDeCompra { get; set; }
    }
}
