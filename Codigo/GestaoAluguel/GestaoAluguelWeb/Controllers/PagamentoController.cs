using GestaoAluguelWeb.Models;
using Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Service;
using AutoMapper;

namespace GestaoAluguelWeb.Controllers
{
    public class PagamentoController : Controller
    {
        private readonly IPagamentoService pagamentoService;
        private readonly ILocacaoService locacaoService;
        private readonly IMapper mapper;

        public PagamentoController(IPagamentoService pagamentoService, ILocacaoService locacaoService, IMapper mapper)
        {
            this.pagamentoService = pagamentoService;
            this.locacaoService = locacaoService;
            this.mapper = mapper;
        }


        // GET: PagamentoController
        public ActionResult Index()
        {
            var pagamentos = pagamentoService.GetAll();
            var pagamentosModel = mapper.Map<List<PagamentoModel>>(pagamentos);
            return View(pagamentosModel);
        }

        // GET: PagamentoController/Details/5
        public ActionResult Details(int id)
        {
            var pagamento = pagamentoService.Get(id);
            var pagamentoModel = mapper.Map<PagamentoModel>(pagamento);
            return View(pagamentoModel);
        }

        // GET: PagamentoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PagamentoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PagamentoModel pagamentoModel)
        {
            if (ModelState.IsValid)
            {
                var pagamento = mapper.Map<Core.Pagamento>(pagamentoModel);
                pagamentoService.Create(pagamento);
            }
            return View(pagamentoModel);
        }

        // GET: PagamentoController/Edit/5
        public ActionResult Edit(int id)
        {
            var pagamento = pagamentoService.Get(id);
            var pagamentoModel = mapper.Map<PagamentoModel>(pagamento);
            return View(pagamentoModel);
        }

        // POST: PagamentoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PagamentoModel pagamentoModel)
        {
            if(ModelState.IsValid)
            {
                var pagamento = mapper.Map<Core.Pagamento>(pagamentoModel);
                pagamentoService.Edit(pagamento);
            }
            return View(pagamentoModel);
        }

        // GET: PagamentoController/Delete/5
        public ActionResult Delete(int id)
        {
            var pagamento = pagamentoService.Get(id);
            var pagamentoModel = mapper.Map<PagamentoModel>(pagamento);
            return View(pagamentoModel);
        }

        // POST: PagamentoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PagamentoModel pagamentoModel)
        {
            try
            {
                pagamentoService.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(pagamentoModel);
            }
        }
    }
}
