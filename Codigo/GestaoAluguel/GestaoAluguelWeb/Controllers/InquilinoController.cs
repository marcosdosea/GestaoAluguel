using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Mvc;


namespace GestaoAluguelWeb.Controllers
{
    public class InquilinoController : Controller
    {
        private readonly IPessoaService inquilinoService;
        private readonly IMapper mapper;

        public InquilinoController(IPessoaService inquilinoService, IMapper mapper)
        {
            this.inquilinoService = inquilinoService;
            this.mapper = mapper;
        }

        // GET: InquilinoController
        public ActionResult Index()
        {
            var listaInquilinos = inquilinoService.GetAll();
            var listaInquilinosModel = mapper.Map<List<InquilinoModel>>(listaInquilinos);
            return View(listaInquilinosModel);
        }

        // GET: InquilinoController/Details/5
        public ActionResult Details(int id)
        {
            var inquilino = inquilinoService.Get(id);
            var inquilinoModel = mapper.Map<InquilinoModel>(inquilino);
            return View(inquilinoModel);
        }


        // GET: InquilinoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InquilinoModel inquilinoModel)
        {
            if (ModelState.IsValid)
            {
                var inquilino = mapper.Map<Pessoa>(inquilinoModel);
                inquilinoService.Create(inquilino);
            }
            return View(inquilinoModel);
        }

        // GET: InquilinoController/Edit/5
        public ActionResult Edit(int id)
        {
            var inquilino = inquilinoService.Get(id);
            var inquilinoModel = mapper.Map<InquilinoModel>(inquilino);
            return View(inquilinoModel);
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InquilinoModel inquilinoModel)
        {
            if (ModelState.IsValid)
            {
                var inquilino = mapper.Map<Pessoa>(inquilinoModel);
                inquilinoService.Edit(inquilino);
            }
            return View(inquilinoModel);
        }

        // GET: InquilinoController/Delete/5
        public ActionResult Delete(int id)
        {
            var inquilino = inquilinoService.Get(id);
            var inquilinoModel = mapper.Map<InquilinoModel>(inquilino);
            return View(inquilinoModel);
        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, InquilinoModel inquilinoModel)
        {
            try
            {
                inquilinoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(inquilinoModel);
            }
        }

    }
}
