using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Helpers;
using GestaoAluguelWeb.Models;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Service;

namespace GestaoAluguelWeb.Controllers
{
    [Authorize]
    public class ImovelController : Controller
    {

        private readonly IImovelService ImovelService;
        private readonly ILocacaoService LocacaoService;
        private readonly IPessoaService PessoaService;
        private readonly IMapper mapper;
        private readonly FileHelper.FileType[] tiposPermitidos = new[] {
            FileHelper.FileType.Jpeg,
            FileHelper.FileType.Png,
            FileHelper.FileType.Bmp

        };
        public ImovelController(IImovelService imovelService, ILocacaoService LocacaoService,IPessoaService PessoaService, IMapper mapper)
        {
            this.ImovelService = imovelService;
            this.LocacaoService = LocacaoService;
            this.PessoaService = PessoaService;
            this.mapper = mapper;
        }

        private int GetPessoaIdLogada()
        {
            var claimId = User.FindFirst("PessoaId")?.Value;
            return claimId != null ? int.Parse(claimId) : 0;
        }

        // GET: ImovelController
        public ActionResult Index()
        {
            var pessoa = PessoaService.Get(GetPessoaIdLogada());

            if (pessoa == null)
            {
                // Se não achou a pessoa, manda cadastrar o perfil primeiro
                return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
            }
            var listaMeusImoveis = new List<ImovelModel>();

            // 1. Buscar imóveis onde sou PROPRIETÁRIO
            var imoveisProprios = ImovelService.GetByProprietario(pessoa.Id);

            foreach (var imovel in imoveisProprios)
            {

                ImovelModel modelo = mapper.Map<ImovelModel>(imovel);
                if (modelo != null) {

                    modelo.Papel = PapelUsuario.Proprietario;
                    listaMeusImoveis.Add(modelo);

                }
                    
            }

            // 2. Buscar imóveis onde sou INQUILINO
            var minhasLocacoes = ImovelService.GetByInquilino(pessoa.Id);

            foreach (var imovel in minhasLocacoes)
            {

                ImovelModel modelo = mapper.Map<ImovelModel>(imovel);
                if (modelo != null)
                {

                    modelo.Papel = PapelUsuario.Inquilino;
                    listaMeusImoveis.Add(modelo);

                }

            }

            return View(listaMeusImoveis);
        }

        // GET: ImovelController/Details/5
        public ActionResult Details(int id)
        {
            var imovel = ImovelService.Get(id);
            if (imovel == null)
            {
                return NotFound();
            }

            // 2. Identifica quem está tentando acessar
            int usuarioLogadoId = GetPessoaIdLogada();

            // 3. VERIFICAÇÃO DE SEGURANÇA (A Mágica acontece aqui!)

            // A) É o Proprietário?
            bool ehDono = imovel.IdProprietario == usuarioLogadoId;

            // B) É o Inquilino? (Tem alguma locação desse imóvel com esse usuário?)
            // Nota: Dependendo da regra, você pode adicionar .Where(l => l.Ativo) se quiser só inquilinos atuais
            bool ehInquilino = LocacaoService.GetAtivasByInquilino(usuarioLogadoId).Any(l => l.IdImovel == imovel.Id);

            // Se NÃO for dono E NÃO for inquilino -> Bloqueia!
            if (!ehDono && !ehInquilino)
            {
                // Retorna erro 403 (Proibido) ou redireciona para uma página amigável
                return Forbid();
            }

            // 4. Se passou, carrega os dados e mostra a tela
            ImovelModel imovelModel = mapper.Map<ImovelModel>(imovel);
            if (ehDono)
            {
                imovelModel.Papel = PapelUsuario.Proprietario;
            }
            else
            {
                imovelModel.Papel = PapelUsuario.Inquilino;
            }
                
            return View(imovelModel);
        }

        // GET: ImovelController/Create
        public ActionResult Create()
        {
            ImovelModel imovelModel = new ImovelModel();


            return View(imovelModel);
        }

        // POST: ImovelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ImovelModel imovelModel, IFormFile? fotoFile)
        {
            if (ModelState.IsValid)
            {
                var imovel = mapper.Map<Imovel>(imovelModel);
                if (fotoFile != null && fotoFile.Length > 0)
                {

                    // --- USANDO O MÉTODO IsValid ---
                    if (!FileHelper.IsValid(fotoFile, tiposPermitidos))
                    {
                        ModelState.AddModelError("Foto", "Tipo de arquivo inválido. Apenas  JPG, PNG e Bmp são permitidos.");
                        return View(imovelModel);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        imovel.Foto = await FileHelper.ConverterParaBytes(fotoFile);   // Converte para byte[] e atribui ao seu modelo
                    }
                }
                imovel.IdProprietario = GetPessoaIdLogada(); // Garante que o imóvel criado seja sempre do usuário logado
                imovel.EstaAlugado = 0; // Novo imóvel começa como disponível
                imovel.Ativo = 1;
                ImovelService.Create(imovel);
                return RedirectToAction(nameof(Index));
            }
            return View(imovelModel);
        }

        // GET: ImovelController/Edit/5
        public ActionResult Edit(int id)
        {
            Imovel? imovel = ImovelService.Get(id);
            if (imovel == null) return NotFound();

            // SEGURANÇA EM UMA LINHA:
            // Verifica se o ID do dono do imóvel é igual ao ID que está no Cookie
            if (imovel.IdProprietario != GetPessoaIdLogada())
            {
                return Forbid(); // Ou Redirect para página de "Sem Permissão"
            }

            ImovelModel imovelModel = mapper.Map<ImovelModel>(imovel);
            return View(imovelModel);
        }

        // POST: ImovelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, ImovelModel imovelModel, IFormFile? fotoFile)
        {
            if (ModelState.IsValid)
            {
                // Primeiro, valide se o ID da URL corresponde ao ID do modelo
                if (id != imovelModel.Id)
                {
                    return NotFound();
                }

                var imovel = mapper.Map<Imovel>(imovelModel);

                if (imovel.IdProprietario != GetPessoaIdLogada())
                {
                    return Forbid(); // Ou Redirect para página de "Sem Permissão"
                }

                
                if (fotoFile != null && fotoFile.Length > 0)
                {

                    // --- USANDO O MÉTODO IsValid ---
                    if (!FileHelper.IsValid(fotoFile, tiposPermitidos))
                    {
                        var FotoAntiga = ImovelService.GetFoto(id);
                        if (FotoAntiga != null)
                        {
                            imovelModel.Foto = FotoAntiga;
                        }
                        ModelState.AddModelError("Foto", "Tipo de arquivo inválido. Apenas  JPG, PNG e Bmp são permitidos.");
                        return View(imovelModel);
                    }

                    using (var memoryStream = new MemoryStream())
                    {
                        imovel.Foto = await FileHelper.ConverterParaBytes(fotoFile);   // Converte para byte[] e atribui ao seu modelo
                    }
                }
                imovel.IdProprietario = GetPessoaIdLogada(); // Garante que o imóvel criado seja sempre do usuário logado
                imovel.EstaAlugado = 0; // Novo imóvel começa como disponível
                ImovelService.Edit(imovel);
                return RedirectToAction(nameof(Index));
            }
            return View(imovelModel);
        }

        // GET: ImovelController/Delete/5
        public ActionResult Delete(int id)
        {
            Imovel? imovel = ImovelService.Get(id);
            if (imovel == null) return NotFound();

            // SEGURANÇA EM UMA LINHA:
            // Verifica se o ID do dono do imóvel é igual ao ID que está no Cookie
            if (imovel.IdProprietario != GetPessoaIdLogada())
            {
                return Forbid(); // Ou Redirect para página de "Sem Permissão"
            }
            ImovelModel imovelModel = mapper.Map<ImovelModel>(imovel);
            return View(imovelModel);
        }

        // POST: ImovelController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ImovelModel imovelModel)
        {
            Imovel? imovel = ImovelService.GetComLocacoes(id);
            if (imovel == null) return NotFound();

            // SEGURANÇA EM UMA LINHA:
            // Verifica se o ID do dono do imóvel é igual ao ID que está no Cookie
            if (imovelModel.IdProprietario != GetPessoaIdLogada() || imovel.IdProprietario != GetPessoaIdLogada())
            {
                return Forbid(); // Ou Redirect para página de "Sem Permissão"
            }

            // VERIFICA SE EXISTE ALGUMA LOCAÇÃO ATIVA PARA ESSE IMÓVEL
            // 2. A Lógica Mágica: Tem locação atrelada?
            if (imovel.Locacaos != null && imovel.Locacaos.Any())
            {
                // EXCLUSÃO LÓGICA: Tem histórico! A gente só "esconde" ele do sistema.
                // Miga, se você não tiver o campo "Ativo", crie no model e no banco!
                imovel.Ativo = 0;
                ImovelService.Edit(imovel);
            }
            else
            {
                ImovelService.Delete(id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
