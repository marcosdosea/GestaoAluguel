using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Helpers;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace GestaoAluguelWeb.Controllers
{
    public class FinalizarLocacaoController : Controller
    {
        
        private readonly ILocacaoService locacaoService;
        private readonly IImovelService imovelService;
        private readonly IMapper mapper;

        public FinalizarLocacaoController(ILocacaoService locacaoService, IImovelService imovelService, IMapper mapper)
        {
            this.locacaoService = locacaoService;
            this.imovelService = imovelService;
            this.mapper = mapper;
        }

        // GET: FinalizarLocacaoController
        public ActionResult Index()
        {
            var listaLocacoes = locacaoService.GetAll();
            var listaLocacoesModel = mapper.Map<List<LocacaoModel>>(listaLocacoes);
            return View();
        }

        // GET: FinalizarLocacaoController/Details/5
        public ActionResult Details(int id)
        {
            var locacao = locacaoService.Get(id);
            var locacaoModel = mapper.Map<LocacaoModel>(locacao);
            return View(locacaoModel);
        }

        // GET: FinalizarLocacaoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FinalizarLocacaoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocacaoModel locacaoModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var locacao = mapper.Map<Core.Locacao>(locacaoModel);
                    locacaoService.Create(locacao);
                    return RedirectToAction(nameof(Index));
                }
                return View(locacaoModel);
            }
            catch
            {
                return View();
            }
        }

        // GET: FinalizarLocacaoController/Edit/5
        public ActionResult Edit(int id)
        {
            var locacao = locacaoService.Get(id);
            if (locacao == null)
            {
                return NotFound();
            }
            LocacaoModel locacaoModel = mapper.Map<LocacaoModel>(locacao);
            return View(locacaoModel);
        }

        // POST: FinalizarLocacaoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LocacaoModel locacaoModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var locacao = mapper.Map<Core.Locacao>(locacaoModel);
                    locacaoService.Edit(locacao);
                    return RedirectToAction(nameof(Details));
                }
                return View(locacaoModel);
            }
            catch
            {
                return View();
            }
        }

        // GET: FinalizarLocacaoController/Delete/5
        public ActionResult Delete(int id)
        {
            var locacao = locacaoService.Get(id);
            if (locacao == null)
            {
                return NotFound();
            }
            LocacaoModel locacaoModel = mapper.Map<LocacaoModel>(locacao);
            return View(locacaoModel);
        }

        // POST: FinalizarLocacaoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, LocacaoModel locacaoModel)
        {
            try
            {
                locacaoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }














    }
}
