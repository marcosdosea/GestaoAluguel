namespace Core.Service
{
    public interface IPagamentoService
    {
        int Create(Pagamento pagamento);
        void Edit(Pagamento pagamento);
        void Delete(int id);
        Pagamento? Get(int id);
        IEnumerable<Pagamento> GetAll();
        IEnumerable<Pagamento> GetByLocacao(int idLocacao);
        IEnumerable<Pagamento> GetByInquilino(int idInquilino);
        IEnumerable<Pagamento> GetByImovel(int idImovel);
    }
}
