using AutoMapper;
using Core;
using GestaoAluguelWeb.Models;

namespace GestaoAluguelWeb.Mappers
{
    public class LocacaoProfile : Profile
    {
        public LocacaoProfile() 
        {
            CreateMap<LocacaoModel, Locacao>().ReverseMap();
        }
    }
}
