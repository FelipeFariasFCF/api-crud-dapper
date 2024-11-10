using AutoMapper;
using CrudDapper8.Dto;
using CrudDapper8.Models;

namespace CrudDapper8.Profiles
{
    public class ProfileAutoMapper : Profile
    {
        public ProfileAutoMapper() 
        {
            CreateMap<Usuario, UsuarioListarDto>();
        }
    }
}
