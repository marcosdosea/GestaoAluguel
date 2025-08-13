namespace Core.Service
{
    public interface ICobrancaService
    {
        int Create(Cobranca cobranca);
        void Edit(Cobranca cobranca);
        void Delete(int id);
        Cobranca? Get(int id);
        IEnumerable<Cobranca> GetAll();
        IEnumerable<Cobranca> GetByLocacao(int idLocacao);
    }
}
