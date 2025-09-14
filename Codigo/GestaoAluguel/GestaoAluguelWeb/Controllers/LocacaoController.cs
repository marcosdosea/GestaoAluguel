using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Models;
using GestaoAluguelWeb.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service;

namespace GestaoAluguelWeb.Controllers
{
    public class LocacaoController : Controller
    {

        private readonly ILocacaoService locacaoService;
        private readonly IImovelService imovelService;
        private readonly IMapper mapper;

        public LocacaoController(ILocacaoService locacaoService, IImovelService imovelService, IMapper mapper)
        {
            this.LocacaoService = locacaoService;
            this.imovelService = imovelService;
            this.mapper = mapper;
        }

        // GET: LocacaoController
        public ActionResult Index()
        {
            var locacoes = LocacaoService.GetAll(); // retorna List<Core.Locacao>
            LocacaoModel locacoesModel = mapper.Map<List<LocacaoModel>>(locacoes); // mapeia para List<LocacaoModel>
            return View(locacoesModel); // passa o model correto para a view
        }

        // GET: LocacaoController/Details/5
        public ActionResult Details(int id)
        {
            var locacao = LocacaoService.Get(id);
            LocacaoModel locacaoModel = mapper.Map<LocacaoModel>(locacao);
            return View(locacaoModel);
        }

        // GET: LocacaoController/Create
        public ActionResult Create(int? idImovel)
        {
            var locacaoModel = new LocacaoModel();

            if (idImovel.HasValue)

                locacaoModel.IdImovel = idImovel.Value; 

            return View(locacaoModel);
        }


        public IActionResult VisualizarContrato(int id)
        {
            var locacao = LocacaoService.Get(id);
            if (locacao == null || locacao.Contrato == null)
            {
                return NotFound();
            }

            // --- USANDO O MÉTODO IsValid ---
            // Aqui, permitimos apenas PDF, DOCX, JPEG e PNG.
            var tiposPermitidos = new[] {
                FileHelper.FileType.Pdf,
                FileHelper.FileType.Docx,
                FileHelper.FileType.Jpeg,
                FileHelper.FileType.Png,
                FileHelper.FileType.Bmp,

            };

            if (!FileHelper.IsValid(locacao.Contrato, tiposPermitidos))
            {
                ModelState.AddModelError("arquivo", "Tipo de arquivo inválido. Apenas PDF, DOCX, JPG, PNG e Bmp são permitidos.");
                return View();
            }

            // --- USANDO O MÉTODO GetDataUrlAsync ---
            var viewModel = new ArquivoModel
            {
                DataUrl = await FileSignatureValidator.GetDataUrlAsync(arquivo),
                TipoArquivo = FileSignatureValidator.GetFileType(arquivo)
            };

            return View("VisualizarContrato", viewModel);
        }
		}
        // POST: LocacaoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocacaoModel locacaoModel)
        {
            if (ModelState.IsValid)
            {
                var locacao = mapper.Map<Locacao>(locacaoModel);
                LocacaoService.Create(locacao);

                // Atualiza o imóvel para "alugado"
                var imovel = imovelService.Get(locacaoModel.IdImovel);
                if (imovel != null)
                {
                    imovel.EstaAlugado = 1;
                    imovelService.Edit(imovel);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(locacaoModel);
        }

        // GET: LocacaoController/Edit/5
        public ActionResult Edit(int id)
        {
            Locacao? locacao = LocacaoService.Get(id);
            LocacaoModel locacaoModel = mapper.Map<LocacaoModel>(locacao);

            return View(locacaoModel);
        }

        // POST: LocacaoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(LocacaoModel locacaoModel)
        {
            if (ModelState.IsValid)
            {
                var locacao = mapper.Map<Locacao>(locacaoModel);
                LocacaoService.Edit(locacao);
                return RedirectToAction(nameof(Index));
            }
            return View(locacaoModel);
        }

        [HttpPost]
        public IActionResult Finalizar(int id)
        {
            _locacaoService.FinalizarLocacao(id);
            return RedirectToAction("Index");
        }
    }
}
