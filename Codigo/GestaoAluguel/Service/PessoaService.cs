using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    /// <summary>
    /// Fornece serviços para gerenciar entidades <see cref="Pessoa"/>, incluindo operações de criação, recuperação, atualização e exclusão.
    /// </summary>
    /// <remarks>Este serviço interage com o contexto de dados subjacente para realizar operações em entidades <see cref="Pessoa"/>.
    /// Ele foi projetado para ser utilizado em cenários onde funcionalidades CRUD (Criar, Ler, Atualizar, Excluir) 
    /// são necessárias para o gerenciamento de registros de pessoas.</remarks>
    public class PessoaService : IPessoaService
    {

        private readonly GestaoAluguelContext context;

        /// <summary>
        /// Construtor que inicializa o serviço com o contexto de dados fornecido.
        /// </summary>
        /// <param name="context"></param>
        public PessoaService(GestaoAluguelContext context)
        {
            this.context = context;
        }

        /// <summary>
        ///  Cria uma nova pessoa na base de dados.
        /// </summary>
        /// <param name="pessoa"></param>
        /// <returns>Id da pessoa cadastrada</returns>
        public int Create(Pessoa pessoa)
        {
            context.Add(pessoa);
            context.SaveChanges();
            return pessoa.Id;
        }


        /// <summary>
        /// Remover a entidade <see cref="Pessoa"/> especificada do banco de dados.
        /// </summary>
        /// <remarks>Este método localiza a entidade <see cref="Pessoa"/> pelo ID fornecido e a remove do banco de dados.</remarks>
        /// <param name="id">O ID da entidade <see cref="Pessoa"/> a ser removida.</param>
        public void Delete(int id)
        {
            var pessoa = context.Pessoas.Find(id);
            if (pessoa != null)
            {
                context.Remove(pessoa);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Atualiza a entidade <see cref="Pessoa"/> especificada no banco de dados.
        /// </summary>
        /// <remarks>Este método aplica as alterações à entidade <see cref="Pessoa"/> fornecida e as salva
        /// no banco de dados. Certifique-se de que a entidade a ser atualizada exista no banco de dados antes de chamar este
        /// método.</remarks>
        /// <param name="pessoa">A entidade <see cref="Pessoa"/> a ser atualizada.</param>
        public void Edit(Pessoa pessoa)
        {
            context.Update(pessoa);
            context.SaveChanges();
        }

        /// <summary>
        /// Buscar uma entidade <see cref="Pessoa"/> pelo ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Dados da pessoa</returns>
        public Pessoa? Get(int id)
        {
            return context.Pessoas.FirstOrDefault(p => p.Id == id);
        }

        public Byte[]? GetFoto(int id)
        {
            return context.Pessoas
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => p.Foto)
                .FirstOrDefault();
        }

        /// <summary>
        /// Listar todas as <see cref="Pessoa"/> do banco de dados.
        /// </summary>
        /// <returns>Dados de todas as pessoas do banco</returns>
        public IEnumerable<Pessoa> GetAll()
        {
            return context.Pessoas.AsNoTracking();
        }

        public IEnumerable<PessoaDTO> GetByNome(string nome)
        {
            return from pessoa in context.Pessoas
                   where pessoa.Nome.StartsWith(nome)
                   select new PessoaDTO
                   {
                       Id = pessoa.Id,
                       Nome = pessoa.Nome,
                       Cpf = pessoa.Cpf,
                       Email = pessoa.Email,
                       Telefone = pessoa.Telefone
                   };
        }
        // ADICIONE A IMPLEMENTAÇÃO:
        public Pessoa? GetByCpf(string cpf)
        {
            // Busca a primeira pessoa que tiver o CPF igual ao informado
            // Retorna null se não encontrar ninguém.
            return context.Pessoas
                          .AsNoTracking() // Opcional: Melhora performance apenas para leitura
                          .FirstOrDefault(p => p.Cpf == cpf);
        }

        // Para edição de pessoa
        public Pessoa? GetByEmail(string email)
        {
            return context.Pessoas
                .FirstOrDefault(p => p.Email == email);
        }

        public Pessoa? GetByEmailAsNoTracking(string email)
        {
            return context.Pessoas
                .AsNoTracking()
                .FirstOrDefault(p => p.Email == email);
        }

        public Boolean ExistsPessoaByCpf(string cpf)
        {
            return context.Pessoas
                .Any(p => p.Cpf == cpf);
        }
        public Boolean ExistsPessoaByEmail(string email)
        {
            return context.Pessoas
                          .Any(p => p.Email == email);
        }

        public int? GetIdByIdUsuario(string idUsuario)
        {
            var pessoa = context.Pessoas.FirstOrDefault(p => p.IdUsuario == idUsuario);
            return pessoa != null ? pessoa.Id : -1; // Retorna -1 se não encontrar

        }
    }
}
