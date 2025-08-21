using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestaoAluguelWeb.Controllers
{
    public class ChamadoReparoController : Controller
    {
        private readonly IChamadoReparoService chamadoReparoService;
        private readonly IMapper mapper;

        public ChamadoReparoController(IChamadoReparoService chamadoReparoService, IMapper mapper)
        {
            this.chamadoReparoService = chamadoReparoService;
            this.mapper = mapper;
        }

        // GET: ChamadoReparo
        public IActionResult Index()
        {
            var chamados = chamadoReparoService.GetAll();
            var chamadosModel = mapper.Map<IEnumerable<ChamadoReparoModel>>(chamados);
            return View(chamadosModel);
        }

        // GET: ChamadoReparo/Details/5
        public IActionResult Details(int id)
        {
            var chamado = chamadoReparoService.Get(id);
            if (chamado == null)
            {
                return NotFound();
            }

            var chamadoModel = mapper.Map<ChamadoReparoModel>(chamado);
            return View(chamadoModel);
        }

        // GET: ChamadoReparo/Create
        public IActionResult Create()
        {
            var model = new ChamadoReparoModel();
            return View(model);
        }

        // POST: ChamadoReparo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ChamadoReparoModel model)
        {
            var chamado = mapper.Map<Chamadoreparo>(model);
            var id = chamadoReparoService.Create(chamado);

            return RedirectToAction(nameof(Index));
        }

        // GET: ChamadoReparo/Edit/5
        public IActionResult Edit(int id)
        {
            var chamado = chamadoReparoService.Get(id);
            if (chamado == null)
            {
                return NotFound();
            }

            var model = mapper.Map<ChamadoReparoModel>(chamado);
            return View(model);
        }

        // POST: ChamadoReparo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ChamadoReparoModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var chamado = mapper.Map<Chamadoreparo>(model);
                chamadoReparoService.Edit(chamado);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: ChamadoReparo/PorImovel/5
        public IActionResult PorImovel(int idImovel)
        {
            var chamadosDTO = chamadoReparoService.GetByImovel(idImovel);
            var chamadosModel = mapper.Map<IEnumerable<ChamadoReparoModel>>(chamadosDTO);

            ViewBag.IdImovel = idImovel;

            return View(chamadosModel);
        }

        // POST: ChamadoReparo/Resolver/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Resolver(int id)
        {
            var chamado = chamadoReparoService.ChamadoResolvido(id);

            return RedirectToAction(nameof(Index));
        }

    }
}