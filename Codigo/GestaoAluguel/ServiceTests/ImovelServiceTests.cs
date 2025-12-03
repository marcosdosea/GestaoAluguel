using Core;
using Core.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.Tests
{
    [TestClass()]
    public class ImovelServiceTests
    {
        private GestaoAluguelContext context;
        private IImovelService imovelService;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<GestaoAluguelContext>();
            builder.UseInMemoryDatabase("GestaoAluguel_Imoveis");
            var options = builder.Options;

            context = new GestaoAluguelContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var imoveis = new List<Imovel>
            {
                new Imovel {
                    Id = 1,
                    Apelido = "Casa de Praia",
                    Logradouro = "Rua do Sol",
                    Numero = "100",
                    Bairro = "Atalaia",
                    Cep = "49000-000",
                    Cidade = "Aracaju",
                    Uf = "SE"
                },
                new Imovel {
                    Id = 2,
                    Apelido = "Sítio Recanto",
                    Logradouro = "Estrada Rural",
                    Numero = "km 5",
                    Bairro = "Zona Rural",
                    Cep = "49500-000",
                    Cidade = "Itabaiana",
                    Uf = "SE"
                },
                new Imovel {
                    Id = 3,
                    Apelido = "Apartamento Centro",
                    Logradouro = "Av. Central",
                    Numero = "500",
                    Bairro = "Centro",
                    Cep = "49000-100",
                    Cidade = "Aracaju",
                    Uf = "SE"
                },
            };

            context.AddRange(imoveis);
            context.SaveChanges();

            imovelService = new ImovelService(context);
        }

        [TestMethod()]
        public void CreateTest()
        {
            var novoImovel = new Imovel()
            {
                Id = 4,
                Apelido = "Kitnet Universitária",
                Logradouro = "Rua dos Estudos",
                Numero = "10",
                Bairro = "Jardins",
                Cep = "12345-678",
                Cidade = "Aracaju",
                Uf = "SE"
            };

            imovelService.Create(novoImovel);

            // Assert
            Assert.AreEqual(4, imovelService.GetAll().Count());
            var imovel = imovelService.Get(4);
            Assert.AreEqual("Kitnet Universitária", imovel.Apelido);
            Assert.AreEqual("Rua dos Estudos", imovel.Logradouro);
            Assert.AreEqual("10", imovel.Numero);
        }

        [TestMethod()]
        public void DeleteTest()
        {
            // Act
            imovelService.Delete(2);

            // Assert
            Assert.AreEqual(2, imovelService.GetAll().Count());
            var imovel = imovelService.Get(2);
            Assert.AreEqual(null, imovel);
        }

        [TestMethod()]
        public void EditTest()
        {
            // Act 
            var imovel = imovelService.Get(3);
            imovel.Apelido = "Apartamento Reformado";

            imovelService.Edit(imovel);

            // Assert
            var imovelAtualizado = imovelService.Get(3);
            Assert.IsNotNull(imovelAtualizado);
            Assert.AreEqual("Apartamento Reformado", imovelAtualizado.Apelido);
        }

        [TestMethod()]
        public void GetTest()
        {
            // Act
            var imovel = imovelService.Get(1);

            // Assert
            Assert.IsNotNull(imovel);
            Assert.AreEqual("Casa de Praia", imovel.Apelido);
            Assert.AreEqual("Aracaju", imovel.Cidade); 
        }

        [TestMethod()]
        public void GetAllTest()
        {
            // Act
            var listaImoveis = imovelService.GetAll();

            // Assert
            Assert.IsInstanceOfType(listaImoveis, typeof(IEnumerable<Imovel>));
            Assert.IsNotNull(listaImoveis);
            Assert.AreEqual(3, listaImoveis.Count());

            Assert.AreEqual(3, listaImoveis.First().Id);
            Assert.AreEqual("Apartamento Centro", listaImoveis.First().Apelido);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void GetByProprietarioTest()
        {
            imovelService.GetByProprietario(1);
        }
    }
}