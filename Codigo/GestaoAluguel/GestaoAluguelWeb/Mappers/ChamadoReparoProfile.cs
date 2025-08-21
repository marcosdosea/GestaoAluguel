using AutoMapper;

namespace GestaoAluguelWeb.Mappers
{
    public class ChamadoReparoProfile : Profile
    {

        public ChamadoReparoProfile()
        {
            CreateMap<Core.Chamadoreparo, Models.ChamadoReparoModel>().ReverseMap();
        }
    }
}

