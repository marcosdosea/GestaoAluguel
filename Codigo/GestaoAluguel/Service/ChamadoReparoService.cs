using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class ChamadoReparoService : IChamadoReparoService
    {
        private readonly GestaoAluguelContext context;

        public ChamadoReparoService(GestaoAluguelContext context)
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
            try
            {
                return context.Chamadoreparos.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao listar chamados de reparo.", ex);
            }
        }

        public IEnumerable<ChamadoReparoDTO> GetByImovel(int idImovel)
        {
            try
            {
                var chamados = context.Chamadoreparos
                    .Where(c => c.IdImovel == idImovel)
                    .Select(c => new ChamadoReparoDTO
                    {
                        Id = c.Id,
                        Status = c.Status,
                        DataCadastro = c.DataCadastro,
                        EstaResolvido = c.EstaResolvido,
                        IdImovel = c.IdImovel
                    })
                    .ToList();
                return chamados;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao obter chamados de reparo por imóvel.", ex);
            }
        }

        public Chamadoreparo ChamadoResolvido(int id)
        {
            var chamado = Get(id) ?? throw new ArgumentException($"Chamado com ID {id} não encontrado");

            if (chamado.Status != "R")
            {
                chamado.Status = "R";
                context.SaveChanges();
            }

            return chamado;
        }
    }
}
