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
    public class PagamentoService : IPagamentoService
    {
        private readonly GestaoAluguelContext context;

        public PagamentoService(GestaoAluguelContext context)
        {
            this.context = context;
        }

        public int Create(Pagamento pagamento)
        {
            try
            {
                context.Add(pagamento);
                context.SaveChanges();
                return pagamento.Id;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar pagamento", ex);
            }
        }

        public void Delete(int id)
        {
            try
            {
                var pagamento = context.Pagamentos.Find(id);
                if (pagamento != null)
                {
                    context.Remove(pagamento);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir pagamento", ex);
            }
        }

        public void Edit(Pagamento pagamento)
        {
            try
            {
                context.Update(pagamento);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao editar pagamento.", ex);
            }
        }

        public Pagamento? Get(int id)
        {
            try
            {
                return context.Pagamentos.Find(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao obter pagamento.", ex);
            }
        }

        public IEnumerable<Pagamento> GetAll()
        {
            try
            {
                return context.Pagamentos.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao obter pagamentos.", ex);
            }
        }

        public IEnumerable<Pagamento> GetByImovel(int idImovel)
        {
            try
            {
                var pagamentos = context.Pagamentos
                    .Where(pagamento => pagamento.IdCobrancas
                       .Any(cobranca => cobranca.IdLocacaoNavigation != null && cobranca.IdLocacaoNavigation.IdImovel == idImovel));
                return pagamentos.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao obter pagamentos por imóvel.", ex);
            }
        }
        public IEnumerable<Pagamento> GetByInquilino(int idInquilino)
        {
            try
            {
                var pagamentos = context.Pagamentos
                .Where(pagamento => pagamento.IdCobrancas
                .Any(cobranca => cobranca.IdLocacaoNavigation != null && cobranca.IdLocacaoNavigation.IdInquilino == idInquilino));
                return pagamentos.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao obter pagamentos por inquilino.", ex);
            }
        }
        public IEnumerable<Pagamento> GetByLocacao(int idLocacao)
        {
            try
            {
                var pagamentos = context.Pagamentos
                .Where(pagamento => pagamento.IdCobrancas
                .Any(cobranca => cobranca.IdLocacao == idLocacao));
                return pagamentos.ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao obter pagamentos por locação.", ex);
            }
        }
    }
}
