using AutoMapper;
using PedagioPayPortalUsuario.Data.Dtos;
using PedagioPayPortalUsuario.Data.Dtos.Usuario;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayPortalUsuario.Profiles
{
    public class PlacaProfile : Profile
    {
        public PlacaProfile()
        {
            CreateMap<PLACAs, PlacaDto>();

        }
    }
}
