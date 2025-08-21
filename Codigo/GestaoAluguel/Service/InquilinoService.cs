using Core;
using Core.DTO;
using Core.Service;
using Microsoft.EntityFrameworkCore;

namespace Service
{
    public class InquilinoService : IPessoaService
    {
        private readonly GestaoAluguelContext context;

        public InquilinoService(GestaoAluguelContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Criar um novo inquilino na base de daos
        /// </summary>
        /// <param name="pessoa"></param>
        /// <returns> id do inquilino</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public int Create(Pessoa pessoa)
        {
            if (pessoa == null)
            {
                throw new ArgumentNullException(nameof(pessoa), "Pessoa não pode ser nula.");
            }
            if (pessoa.Nascimento > DateTime.Now)
            {
                throw new ArgumentException("Data de nascimento não pode ser no futuro.", nameof(pessoa.Nascimento));
            }
            if (pessoa.Nascimento < DateTime.Now.AddYears(-120))
            {
                throw new ArgumentException("Data de nascimento inválida.", nameof(pessoa.Nascimento));
            }

            context.Pessoas.Add(pessoa);
            context.SaveChanges();
            return pessoa.Id;
        }

        public void Edit(Pessoa pessoa)
        {
            if (pessoa == null)
            {
                throw new ArgumentNullException(nameof(pessoa), "Pessoa não pode ser nula.");
            }
            if (pessoa.Nascimento > DateTime.Now)
            {
                throw new ArgumentException("Data de nascimento não pode ser no futuro.", nameof(pessoa.Nascimento));
            }
            if (pessoa.Nascimento < DateTime.Now.AddYears(-120))
            {
                throw new ArgumentException("Data de nascimento inválida.", nameof(pessoa.Nascimento));
            }
            context.Pessoas.Update(pessoa);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var pessoa = Get(id);
            if (pessoa == null)
            {
                throw new ArgumentException("Pessoa não encontrada.", nameof(id));
            }
            context.Pessoas.Remove(pessoa);
            context.SaveChanges();
        }

        public Pessoa? Get(int id)
        {
            return context.Pessoas.Find(id);
        }

        public IEnumerable<Pessoa> GetAll()
        {
            return context.Pessoas.AsNoTracking();
        }

        public IEnumerable<PessoaDTO> GetByNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("Nome não pode ser nulo ou vazio.", nameof(nome));
            }
            return context.Pessoas
                .Where(p => p.Nome.Equals(nome))
                .Select(p => new PessoaDTO
                {
                    Id = p.Id,
                    Nome = p.Nome,
                    Cpf = p.Cpf,
                    Email = p.Email,
                    Telefone = p.Telefone
                })
                .AsNoTracking()
                .ToList();
        }




    }
}
