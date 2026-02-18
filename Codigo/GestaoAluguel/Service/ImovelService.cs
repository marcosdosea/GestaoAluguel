using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;
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

        public IEnumerable<Imovel> GetByProprietario(int idProprietario)
        {
            return context.Imovels
                .Where(imovel => imovel.IdProprietario == idProprietario)
                .OrderBy(imovel => imovel.Apelido)
                .ToList();
        }

        public IEnumerable<Imovel> GetByInquilino(int idInquilino)
        {
            return context.Imovels
                .Where(Imovel => (
                        context.Locacaos
                       .Where(locacao => locacao.IdInquilino == idInquilino 
                              && locacao.IdImovel == Imovel.Id
                              && locacao.Status == 1).AsNoTracking().ToList()).Count > 0)
                .OrderBy(imovel => imovel.Apelido)
                .ToList();
        }

        public Byte[]? GetFoto(int id)
        {
            return context.Imovels
                .AsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => i.Foto)
                .FirstOrDefault();
        }

    }
}
