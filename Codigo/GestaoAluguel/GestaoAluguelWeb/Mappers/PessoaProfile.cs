using AutoMapper;

namespace GestaoAluguelWeb.Mappers
{
    public class PessoaProfile : Profile
    {

        public PessoaProfile()
        {
            CreateMap<Core.Pessoa, Models.PessoaModel>().ReverseMap();
        }
    }
}
