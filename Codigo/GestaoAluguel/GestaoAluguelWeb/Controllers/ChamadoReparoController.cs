using AutoMapper;
using Core;
using Core.Service;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service;

namespace GestaoAluguelWeb.Controllers
{
    public class ChamadoReparoController : Controller
    {
        private readonly IChamadoReparoService chamadoReparoService;
        private readonly IPessoaService PessoaService;
        private readonly IImovelService ImovelService;
        private readonly ILocacaoService LocacaoService;
        private readonly IMapper mapper;

        public ChamadoReparoController(IChamadoReparoService chamadoReparoService, IPessoaService PessoaService, IImovelService imovelService, ILocacaoService locacaoService ,IMapper mapper)
        {
            this.ImovelService = imovelService;
            this.chamadoReparoService = chamadoReparoService;
            this.PessoaService = PessoaService;
            this.LocacaoService = locacaoService;
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
            
            chamadosModel.ToList().ForEach(chamadoModel =>
            {
                var chamado = chamados.First(c => c.Id == chamadoModel.Id);
                chamadoModel.ApelidoImovel = chamado.IdImovelNavigation.Apelido;
            });
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
            chamadoModel.EnderecoImovel = chamado.IdImovelNavigation.Logradouro + ", " + chamado.IdImovelNavigation.Numero + " - " + chamado.IdImovelNavigation.Bairro;
            chamadoModel.ApelidoImovel = chamado.IdImovelNavigation.Apelido;
            return View(chamadoModel);
        }

        // GET: ChamadoReparo/Create
        public IActionResult Create(int? idImovel)
        {
            var imoveis = ImovelService.GetByUsuarioComLocacaoAtiva(GetPessoaIdLogada());
           
            // 2. Cria o SelectList. 
            // O 1º parâmetro é a lista.
            // O 2º é qual campo será o "Value" (o Id).
            // O 3º é o que vai aparecer pro usuário (ex: Nome, Rua, etc).
            // O 4º parâmetro é o pulo do gato: o idImovel que veio da URL! Se ele vier preenchido, o Dropdown já vem selecionado nele.
            ViewBag.IdImovel = new SelectList(imoveis, "Id", "Apelido", idImovel);

            var model = new ChamadoReparoModel();
            return View(model);
        }

        // POST: ChamadoReparo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ChamadoReparoModel model)
        {

            if (model.IdImovel == 0)
            {
                ModelState.AddModelError("IdImovel", "O campo Imóvel é obrigatório.");
                return View(model);
            }

            var idPessoaLogada = GetPessoaIdLogada();
            var chamado = mapper.Map<Chamadoreparo>(model);
            var imoveisLocados = ImovelService.GetByInquilino(idPessoaLogada).ToList();
            var imoveisPossuidos = ImovelService.GetByProprietarioComLocacaoAtiva(idPessoaLogada).ToList();

            if(imoveisLocados.Count == 0 && imoveisPossuidos.Count == 0) {
                Forbid();
            }

            Boolean pessoaAlugouImovel = imoveisLocados.Any(i => i.Id == chamado.IdImovel);
            Boolean pessoaPossuiImovel = imoveisPossuidos.Any(i => i.Id == chamado.IdImovel);

            if (pessoaAlugouImovel) {

                chamado.IdInquilino = idPessoaLogada;
                
            }
            else if (pessoaPossuiImovel) {

                var locacao = LocacaoService.GetByImovel(chamado.IdImovel).OrderByDescending(i => i.DataInicio).First();
                if(locacao != null)
                {

                    chamado.IdInquilino = locacao.IdInquilino;

                }
                else
                {

                    ModelState.AddModelError("IdImovel", "Não é possível abrir chamado para um imóvel desocupado.");

                    // Recarrega o dropdown com os imóveis válidos
                    var imoveisValidos = imoveisPossuidos;
                    imoveisValidos.AddRange(imoveisLocados);
                    ViewBag.IdImovel = new SelectList(imoveisValidos, "Id", "Rua", model.IdImovel);

                    return View(model);

                }

            }
            else
                return Forbid();

            chamado.EstaResolvido = 0;
            chamado.DataCadastro = DateTime.Now;
            chamado.Status = "C";

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

            var idPessoaLogada = GetPessoaIdLogada();

            if (chamado.IdInquilino != idPessoaLogada && chamado.IdImovelNavigation.IdProprietario != idPessoaLogada)
                return Forbid();

            var model = mapper.Map<ChamadoReparoModel>(chamado);

            model.EnderecoImovel = chamado.IdImovelNavigation.Logradouro + ", " + chamado.IdImovelNavigation.Numero + " - " + chamado.IdImovelNavigation.Bairro;
            model.ApelidoImovel = chamado.IdImovelNavigation.Apelido;

            var imoveis = ImovelService.GetByUsuarioComLocacaoAtiva(GetPessoaIdLogada());

            ViewBag.IdImovel = new SelectList(imoveis, "Id", "Apelido", model.IdImovel);


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
                var idPessoaLogada = GetPessoaIdLogada();

                if (model.IdInquilino != idPessoaLogada)
                {
                    var chamadoBanco = chamadoReparoService.Get(id);
                    if (chamadoBanco == null)
                    {
                        return NotFound();
                    }
                    if (chamadoBanco.IdImovelNavigation.IdProprietario != idPessoaLogada)
                    return Forbid();

                }
                    
                var chamado = mapper.Map<Chamadoreparo>(model);
                chamadoReparoService.Edit(chamado);

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: ChamadoReparo/PorImovel/5
        public IActionResult PorImovel(int idImovel)
        {
            var imovel = ImovelService.GetComLocacoes(idImovel);

            if (imovel == null)
            {
                return NotFound();
            }

            var idPessoaLogada = GetPessoaIdLogada();

            if (imovel.IdProprietario != idPessoaLogada && !imovel.Locacaos.Any(l => l.IdInquilino == idPessoaLogada && l.Status == 1))
                return Forbid();

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
            var chamado = chamadoReparoService.Get(id);
            if (chamado == null)
            {
                return NotFound();
            }

            var idPessoaLogada = GetPessoaIdLogada();

            if (chamado.IdInquilino != idPessoaLogada && chamado.IdImovelNavigation.IdProprietario != idPessoaLogada)
                return Forbid();

            if (chamado.Status != "R")
            {
                chamado.Status = "R";
                chamado.DataResolucao = DateTime.Now;
                chamadoReparoService.Edit(chamado);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}