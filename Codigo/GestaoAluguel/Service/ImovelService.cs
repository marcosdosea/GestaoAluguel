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
    class ImovelService : IImovelService
    {
        private readonly GestaoAluguelContext context

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
            var imovel = context.Imoveis.Find(id);
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
            return context.Imoveis.Find(id);
        }

        public IEnumerable<Imovel> GetAll()
        {
            var query = from imovel in context.Imoveis
                        orderby imovel.apelido
                        select imovel;
                        {
                            Id = imovel.Id,
                            Apelido = imovel.Apelido,
                            Logradouro = imovel.Logradouro,
                            EstaAlugado = imovel.EstaAlugado,
                            numero = imovel.Numero
                        }
        }

        public IEnumerable<ImovelDTO> GetByProprietario(int idProprietario)
        {
            throw new NotImplementedException();
        }
    }
}
