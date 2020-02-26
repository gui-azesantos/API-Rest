using System.Text;
using ApiRest.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ApiRest {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext> (options => options.UseMySql (Configuration.GetConnectionString ("DefaultConnection")));
            services.AddControllers ();

            string Token = "Testeabcdefghijklmno_pqrstuvwxyzz"; //Chave de Segurança
            var ChaveSimetrica = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Token));
            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme).AddJwtBearer (options => options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    //Dados de validação Jwt
                    ValidIssuer = "Teste.com",
                    ValidAudience = "User",
                    IssuerSigningKey = ChaveSimetrica
            });

            //Swagger
            services.AddSwaggerGen (config => {
                config.SwaggerDoc ("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "API de Produtos", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            }

            app.UseHttpsRedirection ();
            app.UseAuthentication(); //Aplica Sistema de Autenticação
            app.UseRouting ();

            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
            app.UseSwagger (config => {
                config.RouteTemplate = "guilherme/{documentName}/swagger.json";
            }); //Gerar um arquivo JSON - Swagger.json
            app.UseSwaggerUI (config => { //View HTML do Swagger
                config.SwaggerEndpoint ("/guilherme/v1/swagger.json", "v1 docs");
            });
        }
    }
}