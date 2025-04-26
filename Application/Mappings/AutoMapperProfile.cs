using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LivroRequestDto, Livro>()
                .ForMember(dest => dest.LivroAutores, opt => opt.MapFrom((src, dest) =>
                    src.LivroAutores.Select(id => new LivroAutor { 
                        AutorCodAu = id, 
                        LivroCodl = dest.Codl 
                    }).ToList()
                ))
                .ForMember(dest => dest.LivroAssuntos, opt => opt.MapFrom((src, dest) =>
                    src.LivroAssuntos.Select(id => new LivroAssunto {
                        AssuntoCodAs = id,
                        LivroCodl = dest.Codl
                    }).ToList()
                ));
            CreateMap<Livro, LivroResponseDto>()
                .ForMember(dest => dest.Autores, opt => opt.MapFrom(src =>
                    src.LivroAutores != null
                        ? src.LivroAutores.Select(la => new AutorDto
                            {
                                CodAu = la.AutorCodAu,
                                Nome = la.Autor != null ? la.Autor.Nome : null
                            }).ToList()
                        : new List<AutorDto>()
                ))
                .ForMember(dest => dest.Assuntos, opt => opt.MapFrom(src =>
                    src.LivroAssuntos != null
                        ? src.LivroAssuntos.Select(la => new AssuntoDto
                            {
                                CodAs = la.AssuntoCodAs,
                                Descricao = la.Assunto != null ? la.Assunto.Descricao : null
                            }).ToList()
                        : new List<AssuntoDto>()
                ));
            CreateMap<AutorRequestDto, Autor>();
            CreateMap<Autor, AutorResponseDto>();
            CreateMap<AssuntoRequestDto, Assunto>();
            CreateMap<Assunto, AssuntoResponseDto>();

            CreateMap<BookTransactionRequestDto, BookTransaction>()
                .ForMember(dest => dest.LivroCodl, opt => opt.MapFrom(src => src.LivroCodl))
                .ForMember(dest => dest.CriadoEm, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<BookTransaction, BookTransactionResponseDto>();
        }
    }
}