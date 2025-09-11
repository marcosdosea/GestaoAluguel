using AutoMapper;
using Core;

namespace GestaoAluguelWeb.Mappers
{
    public class CobrancaProfile : Profile
    {
        public CobrancaProfile()
        {
            CreateMap<Cobranca, Models.CobrancaModel>().ReverseMap();
        }
    }
}
