using LB_Gestor_API.Repository.DAO;
using LB_Gestor_API.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace LB_Gestor_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IContaGer, ContaGerDAO>();
            services.AddTransient<IUsuario, UsuarioDAO>();
            services.AddTransient<IHistorico, HistoricoDAO>();
            services.AddTransient<IDuplicata, DuplicataDAO>();
            services.AddTransient<IPlanoConta, PlanoContaDAO>();
            services.AddTransient<IPortador, PortadorDAO>();
            services.AddTransient<ICliente, ClienteDAO>();
            services.AddTransient<ICidade, CidadeDAO>();
            services.AddTransient<IUF, UFDAO>();
            services.AddTransient<ICfgDespAuto, CfgDespAutoDAO>();
            services.AddTransient<ICaixa, CaixaDAO>();
            services.AddTransient<IKPIFaturamento, KPIFaturamentoDAO>();
            services.AddTransient<IRestaurante, RestauranteDAO>();
            services.AddTransient<IRecebimentoVendedor, RecebimentoVendedorDAO>();
            services.AddTransient<IEstoque, EstoqueDAO>();

            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LB_Gestor_API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LB_Gestor_API v1"));
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
