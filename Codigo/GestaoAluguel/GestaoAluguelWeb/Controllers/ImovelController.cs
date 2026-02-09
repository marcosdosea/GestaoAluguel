using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public ImovelController(IImovelService imovelService, IImovelService ImovelService, ILocacaoService LocacaoService,IPessoaService PessoaService, IMapper mapper)
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
            bool ehInquilino = LocacaoService.GetByInquilino(usuarioLogadoId).Any(l => l.IdImovel == imovel.Id);

            // Se NÃO for dono E NÃO for inquilino -> Bloqueia!
            if (!ehDono && !ehInquilino)
            {
                // Retorna erro 403 (Proibido) ou redireciona para uma página amigável
                return Forbid();
            }

            // 4. Se passou, carrega os dados e mostra a tela
            ImovelModel imovelModel = mapper.Map<ImovelModel>(imovel);
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
        public ActionResult Create(ImovelModel imovelModel)
        {
            if (ModelState.IsValid)
            {
                    var imovel = mapper.Map<Imovel>(imovelModel);
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
        public ActionResult Edit(ImovelModel imovelModel)
        {
            if (ModelState.IsValid)
            {
                    var imovel = mapper.Map<Imovel>(imovelModel);
                    ImovelService.Edit(imovel);
                    return RedirectToAction(nameof(Index));
            }
            return View(imovelModel);
        }

        // GET: ImovelController/Delete/5
        public ActionResult Delete(int id)
        {
            var imovel = ImovelService.Get(id);
            ImovelModel imovelModel = mapper.Map<ImovelModel>(imovel);
            return View();
        }

        // POST: ImovelController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ImovelModel imovelModel)
        {
            ImovelService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
