using AutoMapper;
using PedagioPayPortalUsuario.Data.Dtos.Usuario;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayPortalUsuario.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile() 
        { 
            CreateMap<USUARIO, CreateUsuarioDto>();
            CreateMap<CreateUsuarioDto, USUARIO>();
            CreateMap<USUARIO, ReadUsuarioDto>();
            CreateMap<ReadUsuarioDto, USUARIO>();
            CreateMap<USUARIO, UpdateUsuarioDto>();
            CreateMap<UpdateUsuarioDto, USUARIO>();
        }
    }
}
