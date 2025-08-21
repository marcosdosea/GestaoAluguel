using Core.DTO;

namespace Core.Service
{
    public interface IChamadoReparoService
    {
        int Create(Chamadoreparo chamadoreparo);
        void Edit(Chamadoreparo chamadoreparo);
        Chamadoreparo? Get(int id);
        IEnumerable<Chamadoreparo> GetAll();
        IEnumerable<ChamadoReparoDTO> GetByImovel(int idImovel);
        Chamadoreparo ChamadoResolvido(int id);
    }
}
