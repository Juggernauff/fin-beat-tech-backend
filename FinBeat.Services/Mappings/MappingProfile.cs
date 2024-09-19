using AutoMapper;
using FinBeat.DAL.Filters;
using FinBeat.DAL.Models;
using FinBeat.Services.Models;

namespace FinBeat.Services.Mappings
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Creates entity mapping configurations and their dto.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<EntityRequestDto, Entity>();

            CreateMap<Entity, EntityResponseDto>();

            CreateMap<EntityFilterDto, EntityFilter>();
        }
    }
}
