using System.Collections.Generic;

namespace Application.Dtos
{
    public class LivroRelatorioDto
    {
        public LivroRelatorioDto()
        {
            NomeAutorMaisLivros = string.Empty;
            LivroMaisAntigo = string.Empty;
            LivroMaisRecente = string.Empty;
            AnoLivroMaisAntigo = null;
            AnoLivroMaisRecente = null;
            TotalLivros = 0;
            TotalAutores = 0;
            MediaLivrosPorAutor = 0;
            QtdAutorMaisLivros = 0;
            LivrosSemAutores = 0;
            LivrosComMultiplosAutores = 0;
        }

        public int TotalLivros { get; set; }
        public int TotalAutores { get; set; }
        public double MediaLivrosPorAutor { get; set; }
        public string NomeAutorMaisLivros { get; set; }
        public int QtdAutorMaisLivros { get; set; }
        public string LivroMaisAntigo { get; set; }
        public int? AnoLivroMaisAntigo { get; set; }
        public string LivroMaisRecente { get; set; }
        public int? AnoLivroMaisRecente { get; set; }
        public int LivrosSemAutores { get; set; }
        public int LivrosComMultiplosAutores { get; set; }
        public int TotalPaginas { get; set; }
        public double MediaPaginas { get; set; }
    }
}
