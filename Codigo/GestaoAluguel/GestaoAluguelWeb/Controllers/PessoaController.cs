using AutoMapper;
using Core;
using Core.DTO;
using Core.Service;
using GestaoAluguelWeb.Areas.Identity.Data;
using GestaoAluguelWeb.Areas.Identity.Pages.Account;
using GestaoAluguelWeb.Helpers;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service;
using System.IO;


namespace GestaoAluguelWeb.Controllers
{

    [Authorize]
    public class PessoaController : Controller
    {
      
        private readonly IPessoaService pessoaService;
        private readonly IMapper mapper;
        private readonly UserManager<UsuarioIdentity> userManager;
        private readonly SignInManager<UsuarioIdentity> signInManager;

        public PessoaController(IPessoaService pessoaService, IMapper mapper, UserManager<UsuarioIdentity> userManager, SignInManager<UsuarioIdentity> signInManager)
        {
            this.pessoaService = pessoaService;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
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
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PessoaController/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PessoaModel pessoaModel)
        {
            if (ModelState.IsValid)
            {
                // 1. Cria o usuário no banco de dados de Identidade
                var user = new UsuarioIdentity { UserName = pessoaModel.Email, Email = pessoaModel.Email };
                var result = await userManager.CreateAsync(user, pessoaModel.Senha);

                if (result.Succeeded)
                {
                    // 2. Mapeia o ViewModel (PessoaModel) para a Entidade (Pessoa)
                    var pessoa = mapper.Map<Pessoa>(pessoaModel);

                    // 3. ✨ A LIGAÇÃO ACONTECE AQUI! ✨
                    //    Guardamos o ID do usuário recém-criado na nossa entidade Pessoa.
                    pessoa.IdUsuario = user.Id;

                    try
                    {
                        // 4. Salva a entidade Pessoa no banco da aplicação usando o serviço
                        pessoaService.Create(pessoa);

                        // 5. Se tudo deu certo, loga o novo usuário no sistema
                        await signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                    catch
                    {
                        // 6. Se deu erro ao salvar a Pessoa, desfazemos o cadastro do usuário
                        //    para não deixar dados inconsistentes.
                        await userManager.DeleteAsync(user);
                        ModelState.AddModelError("", "Ocorreu um erro ao salvar os dados. Tente novamente.");
                        return View(pessoaModel);
                    }
                }

                // Se a criação do usuário do Identity falhou, mostra os erros
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(pessoaModel);
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
