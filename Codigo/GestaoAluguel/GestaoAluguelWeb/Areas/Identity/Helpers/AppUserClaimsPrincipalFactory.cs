using Core.Service;
using GestaoAluguelWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace GestaoAluguelWeb.Areas.Identity.Helpers
{
    public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<UsuarioIdentity>
    {
        private readonly IPessoaService _pessoaService;

        public AppUserClaimsPrincipalFactory(
            UserManager<UsuarioIdentity> userManager,
            IOptions<IdentityOptions> optionsAccessor,
            IPessoaService pessoaService) // Injetamos seu serviço
            : base(userManager, optionsAccessor)
        {
            _pessoaService = pessoaService;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(UsuarioIdentity user)
        {
            // Gera as claims padrões do Identity
            var identity = await base.GenerateClaimsAsync(user);

            // Busca sua entidade Pessoa baseada no email do login
            var email = await UserManager.GetEmailAsync(user);
            
            if(email == null)
                return identity;

            var pessoa = _pessoaService.GetByEmail(email);

            if (pessoa != null)
            {
                // ADICIONA O ID DA PESSOA NO COOKIE!
                identity.AddClaim(new Claim("PessoaId", pessoa.Id.ToString()));

                // Adiciona o Nome para exibir fácil no Layout
                identity.AddClaim(new Claim("NomeCompleto", pessoa.Nome));

                // Se quiser já definir roles fixas, seria aqui, mas vamos focar no ID primeiro
            }

            return identity;
        }
    }
}
