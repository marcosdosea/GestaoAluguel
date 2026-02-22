using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace GestaoAluguelWeb.Controllers
{
    public class ChamadoReparoController : Controller
    {
        private readonly IChamadoReparoService chamadoReparoService;
        private readonly IPessoaService PessoaService;
        private readonly IImovelService ImovelService;
        private readonly IMapper mapper;

        public ChamadoReparoController(IChamadoReparoService chamadoReparoService, IPessoaService PessoaService, IImovelService imovelService, IMapper mapper)
        {
            this.ImovelService = imovelService;
            this.chamadoReparoService = chamadoReparoService;
            this.PessoaService = PessoaService;
            this.mapper = mapper;
        }

        private int GetPessoaIdLogada()
        {
            var claimId = User.FindFirst("PessoaId")?.Value;
            return claimId != null ? int.Parse(claimId) : 0;
        }

        // GET: ChamadoReparo
        public IActionResult Index()
        {
            var pessoa = PessoaService.Get(GetPessoaIdLogada());
            if (pessoa == null)
            {
                // Se não achou a pessoa, manda cadastrar o perfil primeiro
                return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
            }
            var chamados = chamadoReparoService.GetByPessoa(pessoa.Id);
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

            int usuarioLogadoId = GetPessoaIdLogada();

            if (chamado.IdInquilino != usuarioLogadoId && 
                chamado.IdImovelNavigation.IdProprietario != usuarioLogadoId)
            {
                return Forbid();
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