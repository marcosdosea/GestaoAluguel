using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class ChamadoReparo : IChamadoReparoService
    {
        private readonly GestaoAluguelContext context;

        public ChamadoReparo(GestaoAluguelContext context)
        {
            this.context = context;
        }

        public int Create(Chamadoreparo ChamadoReparo)
        {
            try
            {
                context.Add(ChamadoReparo);
                context.SaveChanges();
                return ChamadoReparo.Id;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar chamado de reparo.", ex);
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Edit(Chamadoreparo ChamadoReparo)
        {
            try
            {
                context.Update(ChamadoReparo);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao editar chamado de reparo.", ex);
            }
        }

        public Chamadoreparo? Get(int id)
        {
            try
            {
                return context.Chamadoreparos.Find(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao buscar chamado de reparo.", ex);
            }
        }

        public IEnumerable<Chamadoreparo> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ChamadoReparoDTO> GetByImovel(int idImovel)
        {
            throw new NotImplementedException();
        }
    }
}
