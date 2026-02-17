using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Helpers;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GestaoAluguelWeb.Areas.Identity.Data;

namespace GestaoAluguelWeb.Controllers
{
    [Authorize]
    public class LocacaoController : Controller
    {
        private readonly ILocacaoService LocacaoService;
        private readonly IImovelService imovelService;
        private readonly IPessoaService pessoaService; // Serviço de Pessoa injetado
        private readonly IMapper mapper;
        private readonly UserManager<UsuarioIdentity> userManager;

        public LocacaoController(ILocacaoService locacaoService, IImovelService imovelService, IPessoaService pessoaService, IMapper mapper, UserManager<UsuarioIdentity> userManager)
        {
            this.LocacaoService = locacaoService;
            this.imovelService = imovelService;
            this.pessoaService = pessoaService;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        // GET: LocacaoController
        public IActionResult Index()
        {
            var locacoes = LocacaoService.GetAll();
            var locacoesModel = mapper.Map<List<LocacaoModel>>(locacoes);
            return View(locacoesModel);
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
                DataUrl = FileHelper.GetDataUrl(locacao.Contrato),
                TipoArquivo = FileHelper.GetFileType(locacao.Contrato)
            };

            return View("VisualizarContrato", viewModel);
        }
        // POST: LocacaoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LocacaoModel locacaoModel)
        {
            // Removemos o erro do IdInquilino do ModelState, pois ele virá 0 e nós vamos resolver isso agora
            ModelState.Remove("IdInquilino");

            // Valida se o resto (Dados da locação + Dados da Pessoa) estão corretos
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Tenta buscar a pessoa pelo CPF digitado na tela
                    var pessoaExistente = pessoaService.GetByCpf(locacaoModel.Inquilino.Cpf);
                    int idPessoaFinal;

                    if (pessoaExistente == null)
                    {
                        // --- CENÁRIO: PESSOA NÃO EXISTE (CRIAR) ---

                        // Mapeia PessoaModel -> Pessoa (Entidade)
                        var novaPessoa = mapper.Map<Pessoa>(locacaoModel.Inquilino);

                        // Garante ID zero para criação
                        novaPessoa.Id = 0;

                        // Salva a nova pessoa
                        pessoaService.Create(novaPessoa);

                        // Pega o ID que o banco gerou
                        idPessoaFinal = novaPessoa.Id;
                    }
                    else
                    {
                        // --- CENÁRIO: PESSOA JÁ EXISTE (USAR ID) ---
                        idPessoaFinal = pessoaExistente.Id;
                    }

                    // 2. Verifica se a pessoa já tem usuário, se não, cria e associa
                    var pessoaParaUsuario = pessoaService.Get(idPessoaFinal);
                    if (pessoaParaUsuario != null && string.IsNullOrEmpty(pessoaParaUsuario.IdUsuario))
                    {
                        UsuarioIdentity usuarioExistente = null;
                        if (!string.IsNullOrEmpty(pessoaParaUsuario.Email))
                        {
                             usuarioExistente = await userManager.FindByEmailAsync(pessoaParaUsuario.Email);
                        }

                        if (usuarioExistente != null)
                        {
                            // CORREÇÃO AUTOMÁTICA: O Login exige que UserName == Email.
                            if (usuarioExistente.UserName != usuarioExistente.Email)
                            {
                                usuarioExistente.UserName = usuarioExistente.Email;
                                await userManager.UpdateNormalizedUserNameAsync(usuarioExistente);
                                await userManager.UpdateAsync(usuarioExistente);
                            }

                            // USUÁRIO JÁ EXISTE: Apenas associa
                            pessoaParaUsuario.IdUsuario = usuarioExistente.Id;
                            pessoaService.Edit(pessoaParaUsuario);
                        }
                        else
                        {
                            // USUÁRIO NÃO EXISTE: Cria novo
                            // Cria usuário com CPF (apenas números)
                            var cpfLimpo = string.Join("", System.Text.RegularExpressions.Regex.Split(pessoaParaUsuario.Cpf, @"[^\d]"));
                            var novoUsuario = new UsuarioIdentity 
                            { 
                                UserName = pessoaParaUsuario.Email, 
                                Email = pessoaParaUsuario.Email,
                                EmailConfirmed = true 
                            };

                            // Usa a senha fornecida no formulário ou uma padrão caso venha nulo (segurança)
                            string senhaParaUsuario = !string.IsNullOrWhiteSpace(locacaoModel.Inquilino.Senha) 
                                ? locacaoModel.Inquilino.Senha 
                                : "Mudar@123"; 

                            var result = await userManager.CreateAsync(novoUsuario, senhaParaUsuario);

                            if (result.Succeeded)
                            {
                                pessoaParaUsuario.IdUsuario = novoUsuario.Id;
                                pessoaService.Edit(pessoaParaUsuario);
                            }
                            else
                            {
                                // Lança exceção para cancelar a transação (se houvesse) e mostrar erro
                                throw new Exception("Erro ao criar usuário automático: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                            }
                        }
                    }

                    // 3. Prepara a Locação
                    var locacao = mapper.Map<Locacao>(locacaoModel);

                    // AQUI ESTÁ O SEGREDOS: Atribuímos o ID da pessoa (nova ou velha) na locação
                    locacao.IdInquilino = idPessoaFinal;

                    LocacaoService.Create(locacao);

                    // 4. Atualiza status do imóvel
                    var imovel = imovelService.Get(locacaoModel.IdImovel);
                    if (imovel != null)
                    {
                        imovel.EstaAlugado = 1;
                        imovelService.Edit(imovel);
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Logar o erro ex
                    ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
                }
            }

            // Se falhar, retorna a view com os dados para corrigir
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

        // GET: LocacaoController/Delete/5
        public ActionResult Delete(int id)
        {
            var locacao = LocacaoService.Get(id);
            LocacaoModel locacaoModel = mapper.Map<LocacaoModel>(locacao);
            return View();
        }

        // POST: LocacaoController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LocacaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}