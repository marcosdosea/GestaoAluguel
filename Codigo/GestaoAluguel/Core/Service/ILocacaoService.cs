using Core.DTO;

namespace Core.Service
{
    public interface ILocacaoService
    {
        int Create(Locacao locacao);
        void Edit(Locacao locacao);
        void Delete(int id);
        Locacao? Get(int id);
        IEnumerable<Locacao> GetAll();
        IEnumerable<LocacaoDTO> GetByImovel(int idImovel);
        IEnumerable<LocacaoDTO> GetByInquilino(int idInquilino);
        IEnumerable<LocacaoDTO> GetAtivasByInquilino(int idInquilino);
        Locacao? GetAtivaByImovel(int idImovel);
    }
}
