using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace DesafioMirante.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tarefa, TarefaDto>().ReverseMap();
            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<Tarefa, TarefaResponseDto>().ReverseMap();
        }
    }
}
