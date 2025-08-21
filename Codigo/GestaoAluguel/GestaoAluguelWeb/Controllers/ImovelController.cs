using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service;

namespace GestaoAluguelWeb.Controllers
{
    public class ImovelController : Controller
    {

        private readonly IImovelService ImovelService;
        private readonly IMapper mapper;

        public ImovelController(IImovelService imovelService, IMapper mapper)
        {
            this.ImovelService = imovelService;
            this.mapper = mapper;
        }
        // GET: ImovelController
        public ActionResult Index()
        {
            var listaImoveis = ImovelService.GetAll(); 
            return View(listaImoveis);
        }

        // GET: ImovelController/Details/5
        public ActionResult Details(int id)
        {
            var imovel = ImovelService.Get(id);
            ImovelModel imovelModel = mapper.Map<ImovelModel>(imovel);
            return View(imovelModel);
        }

        // GET: ImovelController/Create
        public ActionResult Create()
        {
            ImovelModel imovelModel = new();
            
            
            return View(imovelModel);
        }

        // POST: ImovelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ImovelModel imovelModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var imovel = mapper.Map<Imovel>(imovelModel);
                    ImovelService.Create(imovel);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Erro ao criar o imóvel.");
                }
            }
            return View(imovelModel);
        }

        // GET: ImovelController/Edit/5
        public ActionResult Edit(int id)
        {
            Imovel? imovel = ImovelService.Get(id);
            ImovelModel imovelModel = mapper.Map<ImovelModel>(imovel);

            return View(imovelModel);
        }

        // POST: ImovelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ImovelModel imovelModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var imovel = mapper.Map<Imovel>(imovelModel);
                    ImovelService.Edit(imovel);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    ModelState.AddModelError("", "Erro ao editar o imóvel.");
                }
            }
            return View(imovelModel);
        }

        // GET: ImovelController/Delete/5
        public ActionResult Delete(int id)
        {
            var imovel = ImovelService.Get(id);
            ImovelModel imovelModel = mapper.Map<ImovelModel>(imovel);
            return View();
        }

        // POST: ImovelController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ImovelModel imovelModel)
        {
            ImovelService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
