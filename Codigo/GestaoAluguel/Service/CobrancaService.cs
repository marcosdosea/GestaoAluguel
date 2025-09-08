using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CobrancaService : ICobrancaService
    {
        private readonly GestaoAluguelContext context;

        public CobrancaService(GestaoAluguelContext context)
        {
            this.context = context;
        }

        public int Create(Cobranca cobranca)
        {
            if(cobranca.Valor <= 0)
            {
                throw new ArgumentException("O valor da cobrança deve ser maior que zero.");
            }

            context.Cobrancas.Add(cobranca);
            context.SaveChanges();
            return cobranca.Id;
        }

        public void Delete(int id)
        {
            var cobranca = context.Cobrancas.Find(id);
            if (cobranca != null)
            {
                context.Cobrancas.Remove(cobranca);
                context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Cobrança não encontrada.");
            }
        }

        public void Edit(Cobranca cobranca)
        {
           if(cobranca.Valor <= 0)
            {
                throw new ArgumentException("O valor da cobrança deve ser maior que zero.");
            }
            var cobrancaEncontrada = context.Cobrancas.Find(cobranca.Id);
            if (cobrancaEncontrada != null)
            {
                cobrancaEncontrada.Valor = cobranca.Valor;
                cobrancaEncontrada.DataCobranca = cobranca.DataCobranca;
                cobrancaEncontrada.TipoCobranca = cobranca.TipoCobranca;
                cobrancaEncontrada.Descricao = cobranca.Descricao;
                cobrancaEncontrada.IdLocacao = cobranca.IdLocacao;
                context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Cobrança não encontrada.");
            }
        }

        public Cobranca? Get(int id)
        {
           return context.Cobrancas.Find(id);
        }

        public IEnumerable<Cobranca> GetAll()
        {
            return context.Cobrancas.AsNoTracking().ToList();
        }

        public IEnumerable<Cobranca> GetByLocacao(int idLocacao)
        {
            return context.Cobrancas
                .Where(cobranca => cobranca.IdLocacao == idLocacao)
                .AsNoTracking()
                .ToList();
        }
    }
}
