using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestaoAluguelWeb.Controllers
{
    public class PessoaController : Controller
    {

        private readonly IPessoaService pessoaService;
        private readonly IMapper mapper; 

        public PessoaController(IPessoaService pessoaService, IMapper mapper)
        {
            this.pessoaService = pessoaService;
            this.mapper = mapper;
        }

        // GET: PessoaController
        public ActionResult Index()
        {
            var listaPessoas = pessoaService.GetAll();
            var listaPessoasModel = mapper.Map<List<PessoaModel>>(listaPessoas);
            return View(listaPessoasModel);
        }

        // GET: PessoaController/Details/5
        public ActionResult Details(int id)
        {
            var pessoa = pessoaService.Get(id);
            PessoaModel pessoaModel = mapper.Map<PessoaModel>(pessoa);
            return View(pessoaModel);
        }

        // GET: PessoaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PessoaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PessoaModel pessoaModel)
        {

            if (ModelState.IsValid)
            {
                var pessoa = mapper.Map<Pessoa>(pessoaModel);
                pessoaService.Create(pessoa);
            }
            return RedirectToAction(nameof(Index));
            
        }

        // GET: PessoaController/Edit/5
        public ActionResult Edit(int id)
        {
            Pessoa? pessoa = pessoaService.Get(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            PessoaModel pessoaModel = mapper.Map<PessoaModel>(pessoa);
            return View(pessoaModel);
        }

        // POST: PessoaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PessoaModel pessoaModel)
        {
            if (ModelState.IsValid)
            {
                var autor = mapper.Map<Pessoa>(pessoaModel);
                pessoaService.Edit(autor);
            }
            return RedirectToAction(nameof(Index));
            
        }

        // GET: PessoaController/Delete/5
        public ActionResult Delete(int id)
        {
            var pessoa = pessoaService.Get(id);
            PessoaModel pessoaModel = mapper.Map<PessoaModel>(pessoa);
            return View(pessoaModel);
        }

        // POST: PessoaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PessoaModel pessoaModel)
        {
            
            pessoaService.Delete(id);
            return RedirectToAction(nameof(Index));
            
        }
    }
}
