using Domain.Entities;

namespace Application.Dtos;
public class LivroResponseDto
{
    public int Codl { get; set; }
    public string Titulo { get; set; }
    public string Editora { get; set; }
    public int Edicao { get; set; }
    public string AnoPublicacao { get; set; }
    public List<AutorDto> Autores { get; set; }
    public List<AssuntoDto> Assuntos { get; set; }
}

public class AutorDto
{
    public int CodAu { get; set; }
    public string Nome { get; set; }
}

public class AssuntoDto
{
    public int CodAs { get; set; }
    public string Descricao { get; set; }
}

