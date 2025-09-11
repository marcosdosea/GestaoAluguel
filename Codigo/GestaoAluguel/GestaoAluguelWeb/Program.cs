using Core;
using Core.Service;
using GestaoAluguelWeb.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service;

namespace GestaoAluguelWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var GestaoAluguelDatabase = builder.Configuration.GetConnectionString("GestaoAluguelConnection");
            if (string.IsNullOrWhiteSpace(GestaoAluguelDatabase))
            {
                throw new InvalidOperationException("Não é possível acessar banco de dados.");
            }

            var IdentityDatabase = builder.Configuration.GetConnectionString("IdentityContext");
            if (string.IsNullOrWhiteSpace(IdentityDatabase))
            {
                throw new InvalidOperationException("Não é possível acessar banco de dados.");
            }


            builder.Services.AddDbContext<GestaoAluguelContext>(options =>
                options.UseMySQL(GestaoAluguelDatabase));

            builder.Services.AddDbContext<IdentityContext>(options =>
               options.UseMySQL(IdentityDatabase));

            builder.Services.AddDefaultIdentity<UsuarioIdentity>(
            options =>
            {
                // SignIn settings
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;

                // Default User settings.
                options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                // Default Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            }).AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>();

            //Configure tokens life
            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromHours(2)
            );

            builder.Services.ConfigureApplicationCookie(options =>
            {
                //options.AccessDeniedPath = "/Identity/Autenticar";
                options.Cookie.Name = "BibliotecaCookieName";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                //options.LoginPath = "/Identity/Autenticar";
                // ReturnUrlParameter requires 
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });

            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddTransient<IPessoaService, PessoaService>();
            builder.Services.AddTransient<IImovelService, ImovelService>();
            builder.Services.AddTransient<IChamadoReparoService, ChamadoReparoService>();
            builder.Services.AddTransient<ILocacaoService, LocacaoService>();
            builder.Services.AddTransient<ICobrancaService, CobrancaService>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Imovel}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
