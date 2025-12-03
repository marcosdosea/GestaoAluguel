using Core;
using Core.Service;
using Service;
using Microsoft.EntityFrameworkCore;

namespace Service.Tests
{
    [TestClass()]
    public class PessoaServiceTests
    {

        private GestaoAluguelContext context;
        private IPessoaService pessoaService;

        [TestInitialize]
        public void Initialize()
        {
            var builder = new DbContextOptionsBuilder<GestaoAluguelContext>();
            builder.UseInMemoryDatabase("GestaoAluguelDB");
            var options = builder.Options;

            context = new GestaoAluguelContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //Criação das entidades de teste

            var pessoas = new List<Pessoa>
            {

                new Pessoa
                {
                    Id = 1,
                    Nome = "Amanda",
                    Cpf = "07834618520",
                    Email = "amandexspeed@gmail.com",
                    Telefone = "79991344707",
                    EstadoCivil = "S",
                    Profissao = "Desenvolvedora",
                    Logradouro = "Rua Vanda Carmen",
                    Uf = "SE",
                    Numero = "1337",
                    Cidade = "Nossa Senhora do Socorro",
                    Bairro = "Piabeta",
                    Nascimento = new DateTime(2005, 03, 10),
                    Cep = "49060-000",
                    NomeConjuge = null

                },
                new Pessoa
                {
                    Id = 2,
                    Nome = "Janizete de Jesus Melo",
                    Cpf = "47880953515",
                    Email = "janizete.melo@gmail.com",
                    Telefone = "79988666598",
                    EstadoCivil = "C",
                    Profissao = "Autônoma",
                    Logradouro = "Rua Vanda Carmen",
                    Uf = "SE",
                    Numero = "1337",
                    Cidade = "Nossa Senhora do Socorro",
                    Bairro = "Piabeta",
                    Nascimento = new DateTime(2005, 03, 10),
                    Cep = "49060-000",
                    NomeConjuge = "Gilvan de Oliveira Melo"

                }

            };

            context.AddRange(pessoas);
            context.SaveChanges();
            pessoaService = new PessoaService(context);

        }

        [TestMethod()]
        public void CreateTestValid()
        {

            pessoaService.Create(new Pessoa
            {
                Id = 3,
                Nome = "Gilvan de Oliveira Melo",
                Cpf = "33613745534",
                Email = "gilvan.melo@gmail.com",
                Telefone = "79988333818",
                EstadoCivil = "C",
                Profissao = "Aposentado",
                Logradouro = "Rua Vanda Carmen",
                Uf = "SE",
                Numero = "1337",
                Cidade = "Nossa Senhora do Socorro",
                Bairro = "Piabeta",
                Nascimento = new DateTime(2005, 03, 10),
                Cep = "49060-000",
                NomeConjuge = "Janizete de Jesus Melo"

            });

            Assert.AreEqual(3, pessoaService.GetAll().Count());
            var pessoa = pessoaService.Get(3);
            Assert.IsNotNull(pessoa);
            Assert.AreEqual("Gilvan de Oliveira Melo", pessoa.Nome);
            Assert.AreEqual("33613745534", pessoa.Cpf);
            Assert.AreEqual(DateTime.Parse("2005-03-10"),pessoa.Nascimento);

        }

        [TestMethod()]
        public void DeleteTestValid()
        {
            pessoaService.Delete(1);

            Assert.AreEqual(1, pessoaService.GetAll().Count());
            var pessoa = pessoaService.Get(1);
            Assert.IsNull(pessoa);
        }

        [TestMethod()]
        public void EditTestValid()
        {
            var pessoa = pessoaService.Get(1);
            pessoa.Nome = "Amanda de Jesus Melo";
            pessoa.Email = "amandexspeed@hotmail.com";

            pessoaService.Edit(pessoa);

            pessoa = pessoaService.Get(1);
            Assert.IsNotNull(pessoa);
            Assert.AreEqual("Amanda de Jesus Melo", pessoa.Nome);
            Assert.AreEqual("amandexspeed@hotmail.com", pessoa.Email);
        }

        [TestMethod()]
        public void EditTestInvalid()
        {
            var pessoa = pessoaService.Get(1);
            pessoa.Nome = "Amanda de Jesus Melo";
            pessoa.Email = "amandexspeed";

            pessoaService.Edit(pessoa);

            pessoa = pessoaService.Get(1);
            Assert.IsNotNull(pessoa);
            Assert.AreEqual("Amanda", pessoa.Nome);
            Assert.AreEqual("amandexspeed@hotmail.com", pessoa.Email);
        }

        [TestMethod()]
        public void GetTestValid()
        {
            var pessoa = pessoaService.Get(1);
            Assert.IsNotNull(pessoa);
            Assert.AreEqual("Amanda", pessoa.Nome);
            Assert.AreEqual("07834618520", pessoa.Cpf);
            Assert.AreEqual(DateTime.Parse("2005-03-10"), pessoa.Nascimento);

        }

        [TestMethod()]
        public void GetTestInvalid()
        {
            var pessoa = pessoaService.Get(4);
            Assert.IsNull(pessoa);

        }

        [TestMethod()]
        public void GetAllTestValid()
        {
           var pessoas = pessoaService.GetAll();
            Assert.IsNotNull(pessoas);
            Assert.AreEqual(2, pessoas.Count());
            Assert.IsTrue(pessoas.Any(p => p.Nome == "Amanda"));
            Assert.IsTrue(pessoas.Any(p => p.Nome == "Janizete de Jesus Melo"));
        }

        [TestMethod()]
        public void GetByNomeTestValid()
        {
            var pessoas = pessoaService.GetByNome("Amanda");
            Assert.IsNotNull(pessoas);
            Assert.AreEqual(1, pessoas.Count());
            Assert.IsTrue(pessoas.Any(p => p.Nome == "Amanda"));
            Assert.IsTrue(pessoas.Any(p => p.Cpf == "07834618520"));
        }

        [TestMethod()]
        public void GetByNomeTestInvalid()
        {
            var pessoas = pessoaService.GetByNome("Lindinho");
            Assert.AreEqual(0, pessoas.Count());
        }
    }
}