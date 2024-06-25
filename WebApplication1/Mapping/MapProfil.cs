using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Mapping
{
    public class MapProfil : Profile
    {
        public MapProfil()
        {
            CreateMap<LoginDto, AppUser>().ReverseMap();     
            CreateMap<RegisterDto, AppUser>().ReverseMap();
            CreateMap<MessageDto, Chat>().ReverseMap();
            CreateMap<MessageDto,Group>().ReverseMap();
            CreateMap<MessageDto,GroupChat>().ReverseMap();
            CreateMap<MessageDto,GroupUsers>().ReverseMap();
           
        }
    }
}
