using AutoMapper;
using SkylineRealty.API.DTOs.PropertyDTOs;
using SkylineRealty.API.Entities;

namespace SkylineRealty.API.AutoMapperProfiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile() { 

            // Mapeo de PropertyToCreateDto a Property
            CreateMap<PropertyToCreateDto, Property>();

            // Mapeo de PropertyToUpdateDto a Property
            CreateMap<PropertyToUpdateDto, Property>();

        }
    }
}
