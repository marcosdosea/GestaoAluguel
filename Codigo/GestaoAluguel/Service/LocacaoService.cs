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
            //throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            var locacao = context.Locacaos.Find(id);
            if (locacao != null)
            {
                context.Locacaos.Remove(locacao);
                context.SaveChanges();
            }
            //throw new NotImplementedException();
        }

        public void Edit(Locacao locacao)
        {
            
                context.Update(locacao);
                context.SaveChanges();
            //throw new NotImplementedException();
        }

        public Locacao? Get(int id)
        {
            return context.Locacaos.Find(id);
            //throw new NotImplementedException();
        }

        public IEnumerable<Locacao> GetAll()
        {
            return context.Locacaos
                .OrderBy(locacao => locacao.Id)
                .ToList();
            //throw new NotImplementedException();
        }

        public IEnumerable<LocacaoDTO> GetByImovel(int idImovel)
        {
            IEnumerable<Locacao> locacoes = context.Locacaos;

            throw new NotImplementedException();
        }

        public IEnumerable<LocacaoDTO> GetByInquilino(int idInquilino)
        {
            throw new NotImplementedException();
        }
    }
}
