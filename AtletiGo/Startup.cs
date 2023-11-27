using AtletiGo.Core.Repositories;
using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Atletica;
using AtletiGo.Core.Repositories.Evento;
using AtletiGo.Core.Repositories.Modalidade;
using AtletiGo.Core.Repositories.QRCode;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Services.Atleta;
using AtletiGo.Core.Services.Atletica;
using AtletiGo.Core.Services.Evento;
using AtletiGo.Core.Services.Modalidade;
using AtletiGo.Core.Services.QRCode;
using AtletiGo.Core.Services.Usuario;
using AtletiGo.Core.Utils;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace AtletiGo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers();

            var key = Encoding.ASCII.GetBytes(Settings.Secret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AtletiGo",
                    Version = "v1"
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Bearer + token",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                          {
                              Reference = new OpenApiReference
                              {
                                  Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                              }
                          },
                         new string[] {}
                    }
                });
            });

            services.AddTransient<IRepositoryBase, RepositoryBase>();

            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<IAtleticaRepository, AtleticaRepository>();
            services.AddTransient<IQRCodeRepository, QRCodeRepository>();
            services.AddTransient<IModalidadeRepository, ModalidadeRepository>();
            services.AddTransient<IAtletaRepository, AtletaRepository>();
            services.AddTransient<IEventoRepository, EventoRepository>();

            services.AddTransient<UsuarioService>();
            services.AddTransient<AtleticaService>();
            services.AddTransient<QRCodeService>();
            services.AddTransient<ModalidadeService>();
            services.AddTransient<AtletaService>();
            services.AddTransient<EventoService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AtletiGo");
            });

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
