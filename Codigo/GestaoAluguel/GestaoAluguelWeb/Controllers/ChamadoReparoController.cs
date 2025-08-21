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
            try
            {
                var chamados = chamadoReparoService.GetAll();
                var chamadosModel = mapper.Map<IEnumerable<ChamadoReparoModel>>(chamados);
                return View(chamadosModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View(new List<ChamadoReparoModel>());
            }
        }

        // GET: ChamadoReparo/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var chamado = chamadoReparoService.Get(id);
                if (chamado == null)
                {
                    return NotFound();
                }

                var chamadoModel = mapper.Map<ChamadoReparoModel>(chamado);
                return View(chamadoModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
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
            try
            {
                if (ModelState.IsValid)
                {
                    var chamado = mapper.Map<Chamadoreparo>(model);
                    var id = chamadoReparoService.Create(chamado);

                    TempData["SuccessMessage"] = "Chamado criado com sucesso!";
                    return RedirectToAction(nameof(Details), new { id });
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: ChamadoReparo/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var chamado = chamadoReparoService.Get(id);
                if (chamado == null)
                {
                    return NotFound();
                }

                var model = mapper.Map<ChamadoReparoModel>(chamado);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ChamadoReparo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ChamadoReparoModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return BadRequest();
                }

                if (ModelState.IsValid)
                {
                    var chamado = mapper.Map<Chamadoreparo>(model);
                    chamadoReparoService.Edit(chamado);

                    TempData["SuccessMessage"] = "Chamado editado com sucesso!";
                    return RedirectToAction(nameof(Details), new { id });
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: ChamadoReparo/PorImovel/5
        public IActionResult PorImovel(int idImovel)
        {
            try
            {
                var chamadosDTO = chamadoReparoService.GetByImovel(idImovel);
                var chamadosModel = mapper.Map<IEnumerable<ChamadoReparoModel>>(chamadosDTO);

                ViewBag.IdImovel = idImovel;

                return View(chamadosModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ChamadoReparo/Resolver/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Resolver(int id)
        {
            try
            {
                var chamado = chamadoReparoService.ChamadoResolvido(id);
                TempData["SuccessMessage"] = "Chamado marcado como resolvido!";

                // Redireciona para detalhes ou para lista por imóvel
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Erro ao resolver chamado: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

    }
}