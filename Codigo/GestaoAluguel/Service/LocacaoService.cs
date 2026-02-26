using Core;
using Core.DTO;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LocacaoService : ILocacaoService
    {
        private readonly GestaoAluguelContext context;

        public LocacaoService(GestaoAluguelContext context)
        {
            this.context = context;
        }

        public int Create(Locacao locacao)
        {
            context.Locacaos.Add(locacao);
            context.SaveChanges();
            return locacao.Id;
        }

        public void Delete(int id)
        {
            var locacao = context.Locacaos.Find(id);
            if (locacao != null)
            {
                context.Locacaos.Remove(locacao);
                context.SaveChanges();
            }
        }

        public void Edit(Locacao locacao)
        {

            context.Update(locacao);
            context.SaveChanges();
        }

        public Locacao? Get(int id)
        {
            return context.Locacaos.Find(id);
        }

        public IEnumerable<Locacao> GetAll()
        {
            return context.Locacaos
                .OrderBy(locacao => locacao.Id)
                .ToList();
        }

        public IEnumerable<LocacaoDTO> GetByImovel(int idImovel)
        {

            return from locacao in context.Locacaos
                   where locacao.IdImovel == idImovel
                   select new LocacaoDTO
                   {
                       Id = locacao.Id,
                       DataInicio = locacao.DataInicio,
                       DataFim = locacao.DataFim,
                       Status = locacao.Status,
                       IdImovel = locacao.IdImovel,
                       IdInquilino = locacao.IdInquilino
                   };

        }

        public IEnumerable<LocacaoDTO> GetByInquilino(int idInquilino)
        {
            return from locacao in context.Locacaos
                   where idInquilino == locacao.IdInquilino
                   select new LocacaoDTO
                   {
                       Id = locacao.Id,
                       DataInicio = locacao.DataInicio,
                       DataFim = locacao.DataFim,
                       Status = locacao.Status,
                       IdImovel = locacao.IdImovel,
                       IdInquilino = locacao.IdInquilino
                   };
        }

        public IEnumerable<LocacaoDTO> GetAtivasByInquilino(int idInquilino)
        {
            return from locacao in context.Locacaos
                   where locacao.IdInquilino == idInquilino
                   && locacao.Status == 1
                   select new LocacaoDTO
                   {
                       Id = locacao.Id,
                       DataInicio = locacao.DataInicio,
                       DataFim = locacao.DataFim,
                       Status = locacao.Status,
                       IdImovel = locacao.IdImovel,
                       IdInquilino = locacao.IdInquilino
                   };
        }

        public Locacao? GetAtivaByImovel(int idImovel)
        {
            return context.Locacaos
                .FirstOrDefault(l => l.IdImovel == idImovel && l.Status == 1);
        }

    }
}
