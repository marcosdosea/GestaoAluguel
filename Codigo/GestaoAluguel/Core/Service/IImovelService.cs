using Core.DTO;

namespace Core.Service
{
    public interface IImovelService
    {
        int Create(Imovel imovel);
        void Edit(Imovel imovel);
        void Delete(int id);
        Imovel? Get(int id);
        IEnumerable<Imovel> GetAll();
        IEnumerable<ImovelDTO> GetByProprietario(int idProprietario);
    }
}
