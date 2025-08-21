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
    public class ImovelService : IImovelService
    {
        private readonly GestaoAluguelContext context;

        public ImovelService(GestaoAluguelContext context)
        {
            this.context = context;
        }
        public int Create(Imovel imovel)
        {
            context.Add(imovel);
            context.SaveChanges();
            return imovel.Id;
        }

        public void Delete(int id)
        {
            var imovel = context.Imovels.Find(id);
            if (imovel != null)
            {
                context.Remove(imovel);
                context.SaveChanges();
            }
        }

        public void Edit(Imovel imovel)
        {
            context.Update(imovel);
            context.SaveChanges();
        }

        public Imovel? Get(int id)
        {
            return context.Imovels.Find(id);
        }

        public IEnumerable<Imovel> GetAll()
        {
            return context.Imovels
                .OrderBy(imovel => imovel.Apelido)
                .ToList();
        }

        public IEnumerable<ImovelDTO> GetByProprietario(int idProprietario)
        {
            throw new NotImplementedException();
        }
    }
}
