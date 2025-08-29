using Core;
using Core.Service;
using Service;
using Microsoft.EntityFrameworkCore;

namespace ServiceTests
{
    [TestClass()]
    public class InquilinoServiceTests
    {

        private GestaoAluguelContext context;
        private IPessoaService inquilinoService;

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
            var inquilinos = new List<Pessoa>
            {
                new Pessoa
                {
                    Id = 4,
                    Nome = "Luiz Guilherme",
                    Cpf = "86204930516",
                    Email = "ziulguilherme0@gmail.com",
                    Telefone = "79998431635",
                    EstadoCivil = "S",
                    Profissao = "Desenvolvedor",
                    Logradouro = "Rua Jose Mesquita da Silveira",
                    Uf = "SE",
                    Numero = "710",
                    Cidade = "Itabaiana",
                    Bairro = "Centro",
                    Nascimento = new DateTime(2000, 06, 14),
                    Cep = "49500-000",
                    NomeConjuge = null

                },
                new Pessoa
                {
                    Id = 2,
                    Nome = "Lizete Andrade",
                    Cpf = "35869720591",
                    Email = "lizetedoleite@gmail.com",
                    Telefone = "79998375104",
                    EstadoCivil = "V",
                    Profissao = "Aposentada",
                    Logradouro = "Rua Jose Mesquita da Silveira",
                    Uf = "SE",
                    Numero = "643",
                    Cidade = "Itabaiana",
                    Bairro = "Centro",
                    Nascimento = new DateTime(1943, 12, 10),
                    Cep = "49500-000",
                    NomeConjuge = "Heleno Andrade",

                }

            };

            context.AddRange(inquilinos);
            context.SaveChanges();
            inquilinoService = new PessoaService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            inquilinoService.Create(new Pessoa
            {
                Id = 3,
                Nome = "Vitoria Doralice",
                Cpf = "12345678901",
                Email = "vitoriaDoralice@gmail.com",
                Telefone = "79991234567",
                EstadoCivil = "S",
                Profissao = "Estudante",
                Logradouro = "Rua das Flores",
                Uf = "SE",
                Numero = "100",
                Cidade = "Itabaiana",
                Bairro = "Marianga",
                Nascimento = new DateTime(2003, 09, 29),
                Cep = "49500-000",
                NomeConjuge = null
            });

            Assert.AreEqual(3, inquilinoService.GetAll().Count());
            var inquilino = inquilinoService.Get(3);
            Assert.IsNotNull(inquilino);
            Assert.AreEqual("Vitoria Doralice", inquilino?.Nome);
            Assert.AreEqual("12345678901", inquilino?.Cpf);
            Assert.AreEqual(DateTime.Parse("2003, 09, 29"), inquilino?.Nascimento);

        }

        [TestMethod()]
        public void DeleteTest()
        {
            inquilinoService.Delete(1);

            Assert.AreEqual(4, inquilinoService.GetAll().Count());
            var inquilino = inquilinoService.Get(1);
            Assert.IsNull(inquilino);
        }

        [TestMethod()]
        public void EditTest()
        {
            var inquilino = inquilinoService.Get(2);
            inquilino.Nome = "Lizete Andrade";
            inquilino.Email = "lizetedoleite@gmail.com";

            inquilinoService.Edit(inquilino);

            inquilino = inquilinoService.Get(2);
            Assert.IsNotNull(inquilino);
            Assert.AreEqual("Lizete Andrade", inquilino.Nome);
            Assert.AreEqual("lizetedoleite@gmail.com", inquilino.Email);
        }

        [TestMethod()]
        public void GetTest()
        {
            var inquilino = inquilinoService.Get(4);
            Assert.IsNotNull(inquilino);
            Assert.AreEqual("Luiz Guilherme", inquilino.Nome);
            Assert.AreEqual("86204930516", inquilino.Cpf);
            Assert.AreEqual(DateTime.Parse("2000, 06, 14"), inquilino.Nascimento);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            var inquilinos = inquilinoService.GetAll();
            Assert.IsNotNull(inquilinos);
            Assert.AreEqual(1, inquilinos.Count());
            Assert.IsTrue(inquilinos.Any(p => p.Nome == "Luiz Guilherme"));
            Assert.IsTrue(inquilinos.Any(p => p.Nome == "Lizete Andrade"));
        }

        [TestMethod()]
        public void GetByNomeTest()
        {
            var inquilinos = inquilinoService.GetByNome("Lizete");
            Assert.IsNotNull(inquilinos);
            Assert.AreEqual(1, inquilinos.Count());
            Assert.IsTrue(inquilinos.Any(p => p.Nome == "Lizete Andrade"));
            Assert.IsTrue(inquilinos.Any(p => p.Cpf == "35869720591"));
        }

    }
}
