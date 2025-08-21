using AutoMapper;
using Core;
using GestaoAluguelWeb.Models;

namespace GestaoAluguelWeb.Mappers
{
    public class ImovelProfile : Profile
    {
        public ImovelProfile()
        {
            CreateMap<ImovelModel, Imovel>().ReverseMap();
        }
    }
}
