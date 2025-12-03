using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Controllers;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace GestaoAluguelWeb.Tests.Controllers
{
    [TestClass()]
    public class ImovelControllerTests
    {
        private static ImovelController controller;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            var mockService = new Mock<IImovelService>();

            // Configuração do AutoMapper (Simulando o Profile para o teste funcionar isolado)
            IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Imovel, ImovelModel>();
                cfg.CreateMap<ImovelModel, Imovel>();
            }).CreateMapper();

            // Configuração dos Mocks (Comportamentos esperados)
            mockService.Setup(service => service.GetAll())
                .Returns(GetTestImoveis());

            mockService.Setup(service => service.Get(1))
                .Returns(GetTargetImovel());

            mockService.Setup(service => service.Create(It.IsAny<Imovel>()))
                .Verifiable();

            mockService.Setup(service => service.Edit(It.IsAny<Imovel>()))
                .Verifiable();

            mockService.Setup(service => service.Delete(It.IsAny<int>()))
                .Verifiable();

            controller = new ImovelController(mockService.Object, mapper);
        }

        [TestMethod()]
        public void IndexTest_Valido()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            // O Controller original retorna a entidade de domínio na View
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(IEnumerable<Imovel>));

            IEnumerable<Imovel>? lista = (IEnumerable<Imovel>)viewResult.ViewData.Model;
            // Assert
        }

        [TestMethod()]
        public void DetailsTest_Valido()
        {
            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ImovelModel));

            ImovelModel imovelModel = (ImovelModel)viewResult.ViewData.Model;
            Assert.AreEqual("Casa de Praia", imovelModel.Apelido);
            Assert.AreEqual(1, imovelModel.Id);
        }

        [TestMethod()]
        public void CreateTest_Get_Valido()
        {
            // Act
            var result = controller.Create();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ImovelModel));
        }

        [TestMethod()]
        public void CreateTest_Valid()
        {
            // Act
            var result = controller.Create(GetNewImovelModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void CreateTest_Post_Invalid()
        {
            // Arrange
            controller.ModelState.AddModelError("Nome", "Campo requerido");

            // Act
            var result = controller.Create(GetNewImovelModel());

            // Assert
            Assert.AreEqual(1, controller.ModelState.ErrorCount);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void EditTest_Get_Valid()
        {
            // Act
            var result = controller.Edit(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.ViewData.Model, typeof(ImovelModel));

            ImovelModel imovelModel = (ImovelModel)viewResult.ViewData.Model;
            Assert.AreEqual("Casa de Praia", imovelModel.Apelido);
        }

        [TestMethod()]
        public void EditTest_Post_Valid()
        {
            // Act
            var result = controller.Edit(GetTargetImovelModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        [TestMethod()]
        public void DeleteTest_Get_Valid()
        {
            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod()]
        public void DeleteTest_Post_Valid()
        {
            // Act
            var result = controller.Delete(1, GetTargetImovelModel());

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = (RedirectToActionResult)result;
            Assert.IsNull(redirectToActionResult.ControllerName);
            Assert.AreEqual("Index", redirectToActionResult.ActionName);
        }

        // Auxiliares

        private ImovelModel GetNewImovelModel()
        {
            return new ImovelModel
            {
                Id = 4,
                Apelido = "Apartamento Centro",
                Logradouro = "Rua Nova, 50"
            };
        }

        private static Imovel GetTargetImovel()
        {
            return new Imovel
            {
                Id = 1,
                Apelido = "Casa de Praia",
                Logradouro = "Av. Oceano, 100"
            };
        }

        private ImovelModel GetTargetImovelModel()
        {
            return new ImovelModel
            {
                Id = 1,
                Apelido = "Casa de Praia",
                Logradouro = "Av. Oceano, 100"
            };
        }

        private IEnumerable<Imovel> GetTestImoveis()
        {
            return new List<Imovel>
            {
                new Imovel { Id = 1, Apelido = "Casa de Praia" },
                new Imovel { Id = 2, Apelido = "Sítio na Serra" },
                new Imovel { Id = 3, Apelido = "Galpão Industrial" }
            };
        }
    }
}