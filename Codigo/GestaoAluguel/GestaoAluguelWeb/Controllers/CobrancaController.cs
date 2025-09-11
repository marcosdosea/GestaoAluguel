using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Core;
using Core.Service;
using Microsoft.AspNetCore.Http;
using Service;

namespace GestaoAluguelWeb.Controllers
{
    public class CobrancaController : Controller
    {
        private readonly ICobrancaService CobrancaService;
        private readonly IMapper mapper;

        public CobrancaController(ICobrancaService cobrancaService, IMapper mapper)
        {
            this.CobrancaService = cobrancaService;
            this.mapper = mapper;
        }
        // GET: CobrancaController
        public ActionResult Index()
        {
            var listaCobrancas = CobrancaService.GetAll();
            var listaCobrancasModel = mapper.Map<List<CobrancaModel>>(listaCobrancas);
            return View(listaCobrancasModel);
        }

        // GET: CobrancaController/Details/5
        public ActionResult Details(int id)
        {
            var cobranca = CobrancaService.Get(id);
            CobrancaModel cobrancaModel = mapper.Map<CobrancaModel>(cobranca);
            return View(cobrancaModel);
        }

        // GET: CobrancaController/Create
        public ActionResult Create()
        {
            CobrancaModel cobrancaModel = new CobrancaModel();
            return View(cobrancaModel);
        }

        // POST: CobrancaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CobrancaModel cobrancaModel)
        {
            if (ModelState.IsValid)
            {
                var cobranca = mapper.Map<Cobranca>(cobrancaModel);
                CobrancaService.Create(cobranca);
                return RedirectToAction(nameof(Index));
            }
            return View(cobrancaModel);
        }

        // GET: CobrancaController/Edit/5
        public ActionResult Edit(int id)
        {
            Cobranca? cobranca = CobrancaService.Get(id);
            CobrancaModel cobrancaModel = mapper.Map<CobrancaModel>(cobranca);

            return View(cobrancaModel);
        }

        // POST: CobrancaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,CobrancaModel cobrancaModel)
        {
            if (ModelState.IsValid)
            {
                var cobranca = mapper.Map<Cobranca>(cobrancaModel);
                CobrancaService.Edit(cobranca);
                return RedirectToAction(nameof(Index));
            }
            return View(cobrancaModel);
        }

        // GET: CobrancaController/Delete/5
        public ActionResult Delete(int id)
        {
            var cobranca = CobrancaService.Get(id);
            CobrancaModel cobrancaModel = mapper.Map<CobrancaModel>(cobranca);
            return View();
        }

        // POST: CobrancaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, CobrancaModel cobrancaModel )
        {
            CobrancaService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        }
}
