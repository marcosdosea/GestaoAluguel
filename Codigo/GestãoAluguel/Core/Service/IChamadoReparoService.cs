using Core.DTO;

namespace Core.Service
{
    public interface IChamadoReparoService
    {
        int Create(ChamadoReparo chamadoreparo);
        void Edit(ChamadoReparo chamadoreparo);
        void Delete(int id);
        ChamadoReparo? Get(int id);
        IEnumerable<ChamadoReparo> GetAll();
        IEnumerable<ChamadoReparoDTO> GetByImovel(int idImovel);

    }
}
