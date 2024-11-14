using AutoMapper;
using SkylineRealty.API.DTOs.CommentDTOs;
using SkylineRealty.API.Entities;

namespace SkylineRealty.API.AutoMapperProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile() {

            CreateMap<Comment, CommentDto>();

            CreateMap<CommentToCreateDto, Comment>();
        }
    }
}
