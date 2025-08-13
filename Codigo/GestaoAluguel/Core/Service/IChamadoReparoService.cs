using Core.DTO;

namespace Core.Service
{
    public interface IChamadoReparoService
    {
        int Create(Chamadoreparo chamadoreparo);
        void Edit(Chamadoreparo chamadoreparo);
        void Delete(int id);
        Chamadoreparo? Get(int id);
        IEnumerable<Chamadoreparo> GetAll();
        IEnumerable<ChamadoReparoDTO> GetByImovel(int idImovel);

    }
}
