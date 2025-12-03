using AutoMapper;
using Core;
using GestaoAluguelWeb.Models;

namespace GestaoAluguelWeb.Mappers
{
    public class PagamentoProfile : Profile
    {
        public PagamentoProfile()
        {
            CreateMap<Pagamento, PagamentoModel>().ReverseMap();
        }
    }
}
