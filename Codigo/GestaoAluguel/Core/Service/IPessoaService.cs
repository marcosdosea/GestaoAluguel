using Core.DTO;

namespace Core.Service
{
    public interface IPessoaService
    {
        int Create(Pessoa pessoa);
        void Edit(Pessoa pessoa);
        void Delete(int id);
        Pessoa? Get(int id);
        IEnumerable<Pessoa> GetAll();
        IEnumerable<PessoaDTO> GetByNome(string nome);
        Pessoa? GetByCpf(string cpf);
        public Byte[]? GetFoto(int id);
        public Pessoa? GetByEmail(string email);
        public int? GetIdByIdUsuario(string idUsuario);
    }
}
