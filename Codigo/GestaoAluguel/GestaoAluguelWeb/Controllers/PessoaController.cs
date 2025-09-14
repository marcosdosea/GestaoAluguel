using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Helpers;
using GestaoAluguelWeb.Models;
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
        public async Task<ActionResult> Edit(int id, PessoaModel pessoaModel, IFormFile? fotoFile)
        {
            // Primeiro, valide se o ID da URL corresponde ao ID do modelo
            if (id != pessoaModel.Id)
            {
                return NotFound();
            }

            // Lógica para tratar o arquivo enviado
            if (fotoFile != null && fotoFile.Length > 0)
            {

                // --- USANDO O MÉTODO IsValid ---
                var tiposPermitidos = new[] {
                    FileHelper.FileType.Jpeg,
                    FileHelper.FileType.Png,
                    FileHelper.FileType.Bmp

                };

                if (!FileHelper.IsValid(fotoFile, tiposPermitidos))
                {
                    
                    var FotoAntiga = pessoaService.GetFoto(id);
                    if (FotoAntiga != null)
                    {
                        pessoaModel.Foto = FotoAntiga;
                    }
                    ModelState.AddModelError("Foto", "Tipo de arquivo inválido. Apenas  JPG, PNG e Bmp são permitidos.");
                    return View(pessoaModel);
                }

                using (var memoryStream = new MemoryStream())
                {
                    await fotoFile.CopyToAsync(memoryStream); // Copia o conteúdo do arquivo para a memória
                    pessoaModel.Foto = memoryStream.ToArray();      // Converte para byte[] e atribui ao seu modelo
                }
            }
            else
            {
                // Nenhum arquivo novo foi enviado.
                // Para evitar que a foto existente seja apagada, precisamos recarregá-la do banco.
                var FotoAntiga = pessoaService.GetFoto(id);
                if (FotoAntiga != null)
                {
                    pessoaModel.Foto = FotoAntiga;
                }
            }

            if (ModelState.IsValid)
            {
                Pessoa pessoa = mapper.Map<Pessoa>(pessoaModel);
                pessoaService.Edit(pessoa);
                return RedirectToAction(nameof(Details), new { id = pessoaModel.Id });

            }
            return View(pessoaModel);
            
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
