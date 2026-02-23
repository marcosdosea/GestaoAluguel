using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public Imovel? GetComLocacoes(int id)
        {
            return context.Imovels.Include(i => i.Locacaos).Where(i => i.Id == id).First();
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

        public IEnumerable<Imovel> GetByProprietarioComLocacaoAtiva(int idProprietario)
        {
            return context.Imovels
                .Where(imovel => imovel.IdProprietario == idProprietario
                        && imovel.Ativo.Equals(1)
                        && 
                        context.Locacaos
                        .Any(l => l.IdImovel == imovel.Id 
                        && l.Status == 1))
                .OrderBy(imovel => imovel.Apelido)
                .ToList();
        }

        public IEnumerable<Imovel> GetByInquilino(int idInquilino)
        {
            var query = (from imovel in context.Imovels // (Confira se no seu context está Imovels ou Imovel)
                                join locacao in context.Locacaos on imovel.Id equals locacao.IdImovel // (Confira Locacaos ou Locacao)
                                where locacao.Status == 1 && // 1 para locação ativa
                                      (locacao.IdInquilino == idInquilino)
                                      && imovel.Ativo == 1 // Verifica se o imóvel está ativo
                         select imovel).Distinct(); // Distinct garante que o imóvel não apareça duplicado

            // O ToList() é o momento em que a query vai até o banco e puxa a informação filtrada
            return query.ToList(); 
        }

        public IEnumerable<Imovel> GetByUsuarioComLocacaoAtiva(int idUsuario)
        {
            var query = (from imovel in context.Imovels // (Confira se no seu context está Imovels ou Imovel)
                         join locacao in context.Locacaos on imovel.Id equals locacao.IdImovel // (Confira Locacaos ou Locacao)
                         where locacao.Status == 1 && // 1 para locação ativa
                               imovel.Ativo == 1 && // Verifica se o imóvel está ativo
                               (locacao.IdInquilino == idUsuario || imovel.IdProprietario == idUsuario)
                         select imovel).Distinct(); // Distinct garante que o imóvel não apareça duplicado

            // O ToList() é o momento em que a query vai até o banco e puxa a informação filtrada
            return query.ToList();
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
