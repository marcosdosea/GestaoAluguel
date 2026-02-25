using Core.DTO;

namespace Core.Service
{
    public interface IChamadoReparoService
    {
        int Create(Chamadoreparo chamadoreparo);
        void Edit(Chamadoreparo chamadoreparo);
        Chamadoreparo? Get(int id);
        IEnumerable<Chamadoreparo> GetAll();
        IEnumerable<Chamadoreparo> GetByImovel(int idImovel);
        IEnumerable<Chamadoreparo> GetByPessoa(int idImovel);
    }
}
