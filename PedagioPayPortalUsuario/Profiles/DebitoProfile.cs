using AutoMapper;
using PedagioPayPortalUsuario.Data.Dtos.Debitos;
using PedagioPayPortalUsuario.Models;

namespace PedagioPayPortalUsuario.Profiles
{
    public class DebitoProfile : Profile
    {
        public DebitoProfile() 
        { 
            CreateMap<DEBITO, DebitosDto>();
        }
    }
}
