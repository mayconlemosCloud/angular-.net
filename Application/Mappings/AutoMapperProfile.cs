using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<LivroRequestDto, Livro>();
            CreateMap<Livro, LivroResponseDto>();
            CreateMap<AutorRequestDto, Autor>();
            CreateMap<Autor, AutorResponseDto>();
        }
    }
}