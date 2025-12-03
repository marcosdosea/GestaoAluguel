using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Mappers;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GestaoAluguelWeb.Controllers.Tests
{
    [TestClass()]
    public class PessoaControllerTests
    {
        private static PessoaController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IPessoaService>();

            IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile(new PessoaProfile())).CreateMapper();

            mockService.Setup(service => service.GetAll())
                .Returns(GetTestPessoas());
            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetPessoa());
            mockService.Setup(service => service.Edit(It.IsAny<Pessoa>()))
                .Verifiable();
            mockService.Setup(service => service.Create(It.IsAny<Pessoa>()))
                .Verifiable();
            controller = new PessoaController(mockService.Object, mapper);
        }

        private PessoaModel GetNewPessoa()
        {
            return new PessoaModel
            {
                Id = 4,
                Nome = "Ana Paula",
                Cpf = "444.555.666-77",
                Rg = "4567890",
                Email = "ana.paula@email.com",
                Telefone = "(79) 99988-7766",
                EstadoCivil = "S", // Solteira
                Profissao = "Designer",
                Logradouro = "Avenida Beira Mar",
                Uf = "SE",
                Numero = "400",
                Cidade = "Aracaju",
                Bairro = "13 de Julho",
                Nascimento = DateTime.Parse("1995-05-15"),
                Cep = "49020-010",
                NomeConjuge = null,
                Foto = null
            };
        }

        private static Pessoa GetTargetPessoa()
        {
            return new Pessoa
            {
                Id = 1,
                Nome = "Carlos Souza",
                Cpf = "222.333.444-55",
                Email = "carlos.souza@email.com",
                Telefone = "(21) 98765-1122",
                EstadoCivil = "U",
                Profissao = "Advogado",
                Logradouro = "Avenida Copacabana",
                Uf = "RJ",
                Numero = "500",
                Cidade = "Rio de Janeiro",
                Bairro = "Copacabana",
                Nascimento = DateTime.Parse("1989-07-30"),
                Cep = "22020-001"
            };
        }

        private PessoaModel GetTargetPessoaModel()
        {
            return new PessoaModel
            {
                Id = 1,
                Nome = "Carlos Souza",
                Cpf = "222.333.444-55",
                Email = "carlos.souza@email.com",
                Telefone = "(21) 98765-1122",
                EstadoCivil = "U",
                Profissao = "Advogado",
                Logradouro = "Avenida Copacabana",
                Uf = "RJ",
                Numero = "500",
                Cidade = "Rio de Janeiro",
                Bairro = "Copacabana",
                Nascimento = DateTime.Parse("1989-07-30"),
                Cep = "22020-001"
            };
        }

        private IEnumerable<Pessoa> GetTestPessoas()
        {
            return new List<Pessoa>
        {
            new Pessoa
            {
                Id = 1,
                Nome = "Carlos Souza",
                Cpf = "222.333.444-55",
                Email = "carlos.souza@email.com",
                Telefone = "(21) 98765-1122",
                EstadoCivil = "U",
                Profissao = "Advogado",
                Logradouro = "Avenida Copacabana",
                Uf = "RJ",
                Numero = "500",
                Cidade = "Rio de Janeiro",
                Bairro = "Copacabana",
                Nascimento = DateTime.Parse("1989-07-30"),
                Cep = "22020-001"
            },
            new Pessoa
            {
                Id = 2,
                Nome = "João da Silva",
                Cpf = "123.456.789-00",
                Email = "joao.silva@email.com",
                Telefone = "(11) 91234-5678",
                EstadoCivil = "C",
                Profissao = "Engenheiro de Software",
                Logradouro = "Rua das Flores",
                Uf = "SP",
                Numero = "123",
                Cidade = "São Paulo",
                Bairro = "Centro",
                Nascimento = DateTime.Parse("1988-10-20"),
                Cep = "01001-000"
            },
            new Pessoa
            {
                Id = 3,
                Nome = "Mariana Costa",
                Cpf = "333.444.555-66",
                Email = "mariana.costa@email.com",
                Telefone = "(31) 99999-8888",
                EstadoCivil = "D", // Divorciada
                Profissao = "Arquiteta",
                Logradouro = "Rua da Bahia",
                Uf = "MG",
                Numero = "1000",
                Cidade = "Belo Horizonte",
                Bairro = "Lourdes",
                Nascimento = DateTime.Parse("1985-12-01"),
                Cep = "30160-011"
            },
        };
        }

        [TestMethod()]
        public void DetailsTest()
        {
            var result = controller.Details(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaModel));
            PessoaModel pessoaModel = (PessoaModel)viewResult.ViewData.Model;
            Assert.AreEqual("Carlos Souza", pessoaModel.Nome);
            Assert.AreEqual(DateTime.Parse("1989-07-30"), pessoaModel.Nascimento);
        }

        [TestMethod()]
        public void CreateTest_Get()
        {
            var result = controller.Create();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void CreateTest_Post()
        {
            var result = controller.Create(GetNewPessoa());
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void EditTest_Get()
        {
            var result = controller.Edit(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaModel));
            PessoaModel pessoaModel = (PessoaModel)viewResult.ViewData.Model;
            Assert.AreEqual("Carlos Souza", pessoaModel.Nome);
            Assert.AreEqual(DateTime.Parse("1989-07-30"), pessoaModel.Nascimento);
        }

        [TestMethod()]
        public void EditTest_GetInvalid()
        {
            var result = controller.Edit(5);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaModel));
            PessoaModel pessoaModel = (PessoaModel)viewResult.ViewData.Model;
            Assert.AreEqual("Carlos Souza", pessoaModel.Nome);
            Assert.AreEqual(DateTime.Parse("1989-07-30"), pessoaModel.Nascimento);
        }

        [TestMethod()]
        public async Task EditTest_Post()
        {
            var pessoaModel = GetTargetPessoaModel();
            var pessoaId = pessoaModel.Id;
            IFormFile? foto = null; // Deixa explícito que o arquivo é nulo

            var result = await controller.Edit(pessoaId, pessoaModel, foto);


            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult), "O resultado deveria ser um redirecionamento.");

            var redirectToActionResult = (RedirectToActionResult)result;

            Assert.AreEqual("Details", redirectToActionResult.ActionName, "Deveria redirecionar para a action 'Details'.");
            Assert.IsNull(redirectToActionResult.ControllerName, "O redirecionamento deveria ser para o mesmo controller.");
        }

        [TestMethod()]
        public async Task EditTest_PostInvalid()
        {
            var pessoaModel = GetTargetPessoaModel();
            var pessoaId = pessoaModel.Id;
            IFormFile? foto = null; // Deixa explícito que o arquivo é nulo

            pessoaModel.Email = "emailinvalido"; // Definindo um email inválido para simular erro de validação

            var result = await controller.Edit(pessoaId, pessoaModel, foto);


            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult), "O resultado deveria ser um redirecionamento.");

            var redirectToActionResult = (RedirectToActionResult)result;

            Assert.AreEqual("Edit", redirectToActionResult.ActionName, "Deveria redirecionar para a action 'Details'.");
            Assert.IsNull(redirectToActionResult.ControllerName, "O redirecionamento deveria ser para o mesmo controller.");
        }

        [TestMethod()]
        public void DeleteTest_Get()
        {

            var result = controller.Delete(1);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(PessoaModel));
            PessoaModel pessoaModel = (PessoaModel)viewResult.ViewData.Model;
            Assert.AreEqual("Carlos Souza", pessoaModel.Nome);
            Assert.AreEqual(DateTime.Parse("1989-07-30"), pessoaModel.Nascimento);
            
        }

        [TestMethod()]
        public void DeleteTest_Post()
        {
            var result = controller.Delete(1);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult ViewResult = (ViewResult)result;
            Assert.IsInstanceOfType(ViewResult.ViewData.Model, typeof(PessoaModel));

        }

        [TestMethod()]
        public void DeleteTest_PostInvalid()
        {
            var result = controller.Delete(5);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult ViewResult = (ViewResult)result;
            Assert.IsInstanceOfType(ViewResult.ViewData.Model, typeof(PessoaModel));

        }
    }
}