using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Helpers;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace GestaoAluguelWeb.Controllers
{
    public class LocacaoController : Controller
    {
        private readonly ILocacaoService locacaoService;
        private readonly IImovelService imovelService;
        private readonly IMapper mapper;

        public LocacaoController(ILocacaoService locacaoService, IImovelService  imovelService, IMapper mapper)
        {
            this.locacaoService = locacaoService;
            this.imovelService = imovelService;
            this.mapper = mapper;
        }

        // GET: LocacaoController1
        public ActionResult Index()
        {
            var listaLocacoes = locacaoService.GetAll();
            var listaLocacoesModel = mapper.Map<List<LocacaoModel>>(listaLocacoes);
            return View(listaLocacoesModel);
        }

        // GET: LocacaoController1/Details/5
        public ActionResult Details(int id)
        {
            var locacao = locacaoService.Get(id);
            LocacaoModel locacaoModel = mapper.Map<LocacaoModel>(locacao);
            return View(locacaoModel);
        }

        // GET: LocacaoController1/Create
        public ActionResult Create(int? idImovel)
        {
            var locacaoModel = new LocacaoModel();

            if (idImovel.HasValue)

                locacaoModel.IdImovel = idImovel.Value;

            return View(locacaoModel);
        }

        // POST: LocacaoController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(LocacaoModel locacaoModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var locacao = mapper.Map<Locacao>(locacaoModel);
                    locacaoService.Create(locacao);

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
            catch
            {
                return View();
            }
        }



        // GET: LocacaoController1/Edit/5
        public ActionResult Edit(int id)
        {
            Locacao? locacao = locacaoService.Get(id);
            if (locacao == null)
            {
                return NotFound();
            }
            LocacaoModel locacaoModel = mapper.Map<LocacaoModel>(locacao);
            return View(locacaoModel);
        }

        // POST: LocacaoController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, LocacaoModel locacaoModel)
        {
            try
            {
                Locacao locacao = mapper.Map<Locacao>(locacaoModel);
                locacaoService.Edit(locacao);
                return RedirectToAction(nameof(Details), new { id = locacaoModel.Id });
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public IActionResult FinalizarLocacao(int id)
        {
            // Defina dataFim como a data atual e motivo como uma string padrão ou obtenha do usuário
            DateTime dataFim = DateTime.Now;
            string motivo = "Finalização padrão"; // Altere conforme necessário

            locacaoService.FinalizarLocacao(id, dataFim, motivo);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult VisualizarContrato(int id)
        {
            var locacao = locacaoService.Get(id);
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

            var tipoArquivo = FileHelper.GetFileType(locacao.Contrato);
            if (!tiposPermitidos.Contains(tipoArquivo))
            {
                ModelState.AddModelError("arquivo", "Tipo de arquivo inválido. Apenas PDF, DOCX, JPG, PNG e Bmp são permitidos.");
                return View();
            }

            //if (!FileHelper.GetFileType(locacao.Contrato, tiposPermitidos))
            //{
            //    ModelState.AddModelError("arquivo", "Tipo de arquivo inválido. Apenas PDF, DOCX, JPG, PNG e Bmp são permitidos.");
            //    return View();
            //}

            // --- USANDO O MÉTODO GetDataUrlAsync ---
            var viewModel = new ArquivoModel
            {
                DataUrl =  FileHelper.GetDataUrl(locacao.Contrato),
                TipoArquivo = FileHelper.GetFileType(locacao.Contrato)
            };

            return View("VisualizarContrato", viewModel);
        }

        //// GET: LocacaoController1/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: LocacaoController1/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
