using System.Collections.Generic;

namespace Application.Dtos
{
    public class LivroRelatorioDto
    {
        public LivroRelatorioDto()
        {
            LivroMaisAntigo = string.Empty;
            LivroMaisRecente = string.Empty;
            AnoLivroMaisAntigo = null;
            AnoLivroMaisRecente = null;
            TotalLivros = 0;
            TotalAutores = 0;
            LivrosComMultiplosAutores = 0;
            TotalCompras = 0;
        }

        public int TotalLivros { get; set; }
        public int TotalAutores { get; set; }
        public string LivroMaisAntigo { get; set; }
        public string? AnoLivroMaisAntigo { get; set; }
        public string LivroMaisRecente { get; set; }
        public string? AnoLivroMaisRecente { get; set; }
        public int LivrosComMultiplosAutores { get; set; }
        public int TotalCompras { get; set; } 
    }
}
