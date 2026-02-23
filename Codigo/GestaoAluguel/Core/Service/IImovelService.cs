using Core.DTO;

namespace Core.Service
{
    public interface IImovelService
    {
        int Create(Imovel imovel);
        void Edit(Imovel imovel);
        void Delete(int id);
        Imovel? Get(int id);
        Imovel? GetComLocacoes(int id);
        IEnumerable<Imovel> GetAll();
        IEnumerable<Imovel> GetByProprietario(int idProprietario);
        IEnumerable<Imovel> GetByInquilino(int idInquilino);
        IEnumerable<Imovel> GetByProprietarioComLocacaoAtiva(int idProprietario);
        IEnumerable<Imovel> GetByUsuarioComLocacaoAtiva(int idUsuario);
        public Byte[]? GetFoto(int id);
    }
}
