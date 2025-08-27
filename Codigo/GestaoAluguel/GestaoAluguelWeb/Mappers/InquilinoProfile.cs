using AutoMapper;

namespace GestaoAluguelWeb.Mappers
{
    public class InquilinoProfile : Profile
    {

        public InquilinoProfile()
        {
            CreateMap<Core.Pessoa, Models.PessoaModel>().ReverseMap();
        }
    }
}
