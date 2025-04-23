using Domain.Entities;

namespace Application.Dtos;
public class LivroRequestDto
{
    public string Titulo { get; set; }
    public string Editora { get; set; }
    public int Edicao { get; set; }
    public string AnoPublicacao { get; set; }
    public List<int> LivroAutores { get; set; }
    public List<int> LivroAssuntos { get; set; }
}
