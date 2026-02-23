// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Core;
using Core.Service;
using GestaoAluguelWeb.Areas.Identity.Data;
using GestaoAluguelWeb.Helpers;
using GestaoAluguelWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Service;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Util;

namespace GestaoAluguelWeb.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly SignInManager<UsuarioIdentity> _signInManager;
        private readonly IPessoaService _pessoaService; // <--- 1. Injeção do Serviço

        public IndexModel(
            UserManager<UsuarioIdentity> userManager,
            SignInManager<UsuarioIdentity> signInManager,
            IPessoaService pessoaService) // <--- 2. Recebe no construtor
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _pessoaService = pessoaService;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Nome Completo *")]
            [StringLength(45, MinimumLength = 5, ErrorMessage = "O nome deve ter obrigatóriamente entre 5 e 45 caracteres")]
            public string Nome { get; set; } = null!;


            [Display(Name = "CPF *")]
            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            [CPF]
            public string Cpf { get; set; } = null!;

            [Display(Name = "RG")]
            [RG]
            public string Rg { get; set; }


            [Display(Name = "E-mail *")]
            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            [EmailAddress(ErrorMessage = "O e-mail informado não é válido.")]
            public string Email { get; set; } = null!;

            [Display(Name = "Telefone *")]
            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            [TelefoneCelular]
            public string Telefone { get; set; } = null!;

            /// <summary>
            /// S - Solteiro
            /// C - Casado
            /// D - Divorciado
            /// U - União Estável
            /// V - Viúvo
            /// 
            /// </summary>
            [Display(Name = "Estado Civil *")]
            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            public string EstadoCivil { get; set; } = null!;

            [Display(Name = "Profissão *")]
            [StringLength(45, MinimumLength = 1, ErrorMessage = "A profissão é obrigatório e deve ter no máximo 45 caracteres")]
            public string Profissao { get; set; } = null!;

            [Display(Name = "Logradouro *")]
            [StringLength(45, ErrorMessage = "O {0} é obrigatório e deve ter no máximo 45 caracteres")]
            public string Logradouro { get; set; } = null!;

            [Display(Name = "UF *")]
            [Required(ErrorMessage = "O campo {0} é obrigatório.")]
            public string Uf { get; set; } = null!;

            [Display(Name = "Número *")]
            [StringLength(6, MinimumLength = 1, ErrorMessage = "O número da casa é obrigatório deve ter no máximo 6 caracteres")]
            public string Numero { get; set; } = null!;

            [Display(Name = "Cidade *")]
            [StringLength(45, MinimumLength = 1, ErrorMessage = "A cidade é obrigatória e deve ter no máximo 45 caracteres")]
            public string Cidade { get; set; } = null!;

            [Display(Name = "Bairro *")]
            [StringLength(45, MinimumLength = 1, ErrorMessage = "O bairro é obrigatório e deve ter no máximo 45 caracteres")]
            public string Bairro { get; set; } = null!;

            [Display(Name = "Data de Nascimento *")]
            [DataType(DataType.Date, ErrorMessage = "Data inválida")]
            [Required(ErrorMessage = "A {0} é obrigatória.")]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            [Data(120)]
            public DateTime Nascimento { get; set; }

            [Display(Name = "CEP*")]
            [Required(ErrorMessage = "O {0} é obrigatório.")]
            [Cep]
            public string Cep { get; set; } = null!;

            [Display(Name = "Nome do Cônjuge")]
            [StringLength(45, ErrorMessage = "O nome do cônjuge deve ter no máximo 45 caracteres")]
            public string? NomeConjuge { get; set; }

            [Display(Name = "Foto de perfil")]
            public byte[]? Foto { get; set; }

            [Display(Name = "Nova Foto")]
            public IFormFile? ArquivoFoto { get; set; } // Esse recebe o upload da tela

            public string? IdUsuario { get; set; }

        }

        private async Task LoadAsync(UsuarioIdentity user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var email = await _userManager.GetEmailAsync(user); // Pega o email do login

            Username = userName;

            var pessoa = _pessoaService.GetByEmail(email); // Busca a pessoa pelo email do login

            Input = new InputModel
            {
                // --- Campos do Identity (Usuário de Login) ---
                PhoneNumber = phoneNumber,
                Email = email,
                IdUsuario = user.Id, // Já deixamos o ID no jeito para salvar/vincular depois

                // --- Campos da Entidade Pessoa ---
                // O operador '?.' (null conditional) evita erro se 'pessoa' for null
                Nome = pessoa?.Nome,
                Cpf = pessoa?.Cpf,
                Rg = pessoa?.Rg,
                Telefone = pessoa?.Telefone == null ? phoneNumber : pessoa?.Telefone,
                EstadoCivil = pessoa?.EstadoCivil,
                Profissao = pessoa?.Profissao,

                // Endereço
                Logradouro = pessoa?.Logradouro,
                Numero = pessoa?.Numero,
                Bairro = pessoa?.Bairro,
                Cidade = pessoa?.Cidade,
                Uf = pessoa?.Uf,
                Cep = pessoa?.Cep,

                // Dados Pessoais
                // Como Nascimento é DateTime (não aceita null), usamos '??' para definir um valor padrão
                Nascimento = pessoa?.Nascimento ?? DateTime.Now,
                NomeConjuge = pessoa?.NomeConjuge,
                Foto = pessoa?.Foto
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Erro ao carregar usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Erro ao carregar usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Erro inesperado ao tentar definir o número de telefone.";
                    return RedirectToPage();
                }
            }
            // 2. Busca a Pessoa pelo email do usuário logado
            var email = await _userManager.GetEmailAsync(user);

            // Tenta encontrar a pessoa no banco. Se não achar, 'pessoa' será null.
            // (Ajuste o método de busca conforme seu Service, ex: ObterPorEmail(email))
            var pessoa = _pessoaService.GetByEmail(email);

            bool ehNovaPessoa = false;

            if (pessoa == null)
            {
                // Se não existe, instanciamos uma nova
                pessoa = new Pessoa();
                pessoa.Email = email; // Garante o vinculo pelo email
                ehNovaPessoa = true;
            }

            if(pessoa.IdUsuario == null)
                pessoa.IdUsuario = user.Id; // Vincula o ID do Identity se não tiver esse campo

            // 3. AQUI É O PULO DO GATO: Preenchemos o objeto Pessoa com os dados do Input
            // Lógica para tratar o arquivo enviado
            if (Input.ArquivoFoto != null && Input.ArquivoFoto.Length > 0)
            {

                // --- USANDO O MÉTODO IsValid ---
                var tiposPermitidos = new[] {
                FileHelper.FileType.Jpeg,
                FileHelper.FileType.Png,
                FileHelper.FileType.Bmp

            };

                if (!FileHelper.IsValid(Input.ArquivoFoto, tiposPermitidos))
                {
                    ModelState.AddModelError("Foto", "Tipo de arquivo inválido. Apenas  JPG, PNG e Bmp são permitidos.");
                    return Page();
                }

                // Converte e salva na pessoa
                pessoa.Foto = await FileHelper.ConverterParaBytes(Input.ArquivoFoto);

            }
            

            // Dados Pessoais
            pessoa.Nome = Input.Nome;
            pessoa.Cpf = Input.Cpf;
            pessoa.Rg = Input.Rg;
            pessoa.Telefone = Input.PhoneNumber;
            pessoa.EstadoCivil = Input.EstadoCivil;
            pessoa.Profissao = Input.Profissao;
            pessoa.Nascimento = Input.Nascimento;
            pessoa.NomeConjuge = Input.NomeConjuge;

            // Endereço
            pessoa.Logradouro = Input.Logradouro;
            pessoa.Numero = Input.Numero;
            pessoa.Bairro = Input.Bairro;
            pessoa.Cidade = Input.Cidade;
            pessoa.Uf = Input.Uf;
            pessoa.Cep = Input.Cep;

            // 4. Salva no Banco de Dados
            try
            {
                if (ehNovaPessoa)
                {
                    _pessoaService.Create(pessoa);
                }
                else
                {
                    _pessoaService.Edit(pessoa);
                }
            }
            catch (Exception ex)
            {
                // Se der erro (ex: CPF duplicado), mostra na tela e não perde os dados digitados
                StatusMessage = "Erro ao salvar dados: " + ex.Message;
                await LoadAsync(user); // Recarrega para não quebrar a tela
                return Page();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Seu perfil foi atualizado com sucesso!";
            return RedirectToPage();
        }
    }
}
