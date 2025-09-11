using AutoMapper;

namespace GestaoAluguelWeb.Mappers
{
    public class PagamentoProfile : Profile
    {
        public PagamentoProfile()
        {
            CreateMap<Core.Pagamento, Models.PagamentoModel>().ReverseMap();
        }
    }
}
