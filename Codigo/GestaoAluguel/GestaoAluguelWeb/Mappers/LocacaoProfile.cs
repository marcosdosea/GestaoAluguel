using AutoMapper;
using Core;
using GestaoAluguelWeb.Models;

namespace GestaoAluguelWeb.Mappers
{
    public class LocacaoProfile : Profile
    {
        public LocacaoProfile()
        {
            // 1. Mapeamento de Pessoa (Model <-> Entity)
            CreateMap<PessoaModel, Pessoa>().ReverseMap();

            // 2. IDA: Da Tela (Model) para o Banco (Entity)
            CreateMap<LocacaoModel, Locacao>()
                // Mapeia o ID do Model (IdInquilino) para o ID do Banco (IdPessoa)
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdInquilino));
            // REMOVIDO: .ForMember(dest => dest.Pessoa...) pois a propriedade não existe

            // 3. VOLTA: Do Banco (Entity) para a Tela (Model)
            CreateMap<Locacao, LocacaoModel>()
                // Mapeia o ID do Banco (IdPessoa) para o ID do Model (IdInquilino)
                .ForMember(dest => dest.IdInquilino, opt => opt.MapFrom(src => src.Id));
            // REMOVIDO: .ForMember(dest => dest.Inquilino...) pois src.Pessoa não existe
        }
    }
}