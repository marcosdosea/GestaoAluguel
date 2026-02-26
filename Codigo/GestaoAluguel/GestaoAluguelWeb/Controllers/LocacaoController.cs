using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Helpers;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Transactions; // Necessário para TransactionScope

namespace GestaoAluguelWeb.Controllers
{
    [Authorize]
    public class LocacaoController : Controller
    {
        private readonly ILocacaoService LocacaoService;
        private readonly IImovelService imovelService;
        private readonly IPessoaService pessoaService; // Serviço de Pessoa injetado
        private readonly IMapper mapper;

        public LocacaoController(ILocacaoService locacaoService, IImovelService imovelService, IPessoaService pessoaService, IMapper mapper)
        {
            this.LocacaoService = locacaoService;
            this.imovelService = imovelService;
            this.pessoaService = pessoaService;
            this.mapper = mapper;
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
        public ActionResult Create(LocacaoModel locacaoModel)
        {
            // Removemos o erro do IdInquilino do ModelState, pois ele virá 0 e nós vamos resolver isso agora
            ModelState.Remove("IdInquilino");

            // Valida se o resto (Dados da locação + Dados da Pessoa) estão corretos
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
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

                            // Opcional: Se quiser atualizar os dados da pessoa existente com o que foi digitado:
                            // var pessoaAtualizada = mapper.Map(locacaoModel.Inquilino, pessoaExistente);
                            // pessoaService.Edit(pessoaAtualizada);
                        }

                        // 2. Prepara a Locação
                        var locacao = mapper.Map<Locacao>(locacaoModel);

                        // AQUI ESTÁ O SEGREDOS: Atribuímos o ID da pessoa (nova ou velha) na locação
                        locacao.IdInquilino = idPessoaFinal;

                        LocacaoService.Create(locacao);

                        // 3. Atualiza status do imóvel
                        var imovel = imovelService.Get(locacaoModel.IdImovel);
                        if (imovel != null)
                        {
                            imovel.EstaAlugado = 1;
                            imovelService.Edit(imovel);
                        }

                        // 4. Salva tudo
                        transaction.Complete();

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        // Logar o erro ex
                        ModelState.AddModelError("", "Erro ao salvar: " + ex.Message);
                    }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            LocacaoService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Locacao/Encerrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Encerrar(int idImovel)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // 1. Busca a locação ativa para o imóvel
                    var locacao = LocacaoService.GetAtivaByImovel(idImovel);
                    if (locacao == null)
                    {
                        return NotFound("Não foi encontrada uma locação ativa para este imóvel.");
                    }

                    // 2. Atualiza a locação
                    locacao.DataFim = DateTime.Now;
                    locacao.Status = 0; // Inativa
                    LocacaoService.Edit(locacao);

                    // 3. Atualiza o imóvel
                    var imovel = imovelService.Get(idImovel);
                    if (imovel != null)
                    {
                        imovel.EstaAlugado = 0; // Disponível
                        imovelService.Edit(imovel);
                    }

                    transaction.Complete();
                    return RedirectToAction("Index", "Imovel");
                }
                catch (Exception)
                {
                    // Em caso de erro, redireciona de volta para a lista de imóveis
                    return RedirectToAction("Index", "Imovel");
                }
            }
        }
    }
}