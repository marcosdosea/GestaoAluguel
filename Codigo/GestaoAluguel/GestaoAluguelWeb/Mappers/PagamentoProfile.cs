using Core;
using GestaoAluguelWeb.Models;

namespace GestaoAluguelWeb.Mappers
{
    public class PagamentoProfile : Profile
    {
        public PagamentoProfile()
        {
            CreateMap<Core.Pagamento, Models.PagamentoModel>().ReverseMap();
        public PagamentoProfile() 
        {
            CreateMap<PagamentoModel, Pagamento>().ReverseMap();
        }
    }
}
