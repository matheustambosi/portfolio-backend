using AtletiGo.Core.Repositories.Atleta;
using AtletiGo.Core.Repositories.Atletica;
using AtletiGo.Core.Repositories.Evento;
using AtletiGo.Core.Repositories.Modalidade;
using AtletiGo.Core.Repositories.QRCode;
using AtletiGo.Core.Repositories.Usuario;
using AtletiGo.Core.Repositories;
using AtletiGo.Core.Services.Atleta;
using AtletiGo.Core.Services.Atletica;
using AtletiGo.Core.Services.Evento;
using AtletiGo.Core.Services.Modalidade;
using AtletiGo.Core.Services.QRCode;
using AtletiGo.Core.Services.Usuario;
using AtletiGo.Core.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Dapper;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = Encoding.ASCII.GetBytes(Settings.Secret);

builder.Services
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

builder.Services.AddSwaggerGen(c =>
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

builder.Services.AddTransient<IRepositoryBase, RepositoryBase>();

builder.Services.AddTransient<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddTransient<IAtleticaRepository, AtleticaRepository>();
builder.Services.AddTransient<IQRCodeRepository, QRCodeRepository>();
builder.Services.AddTransient<IModalidadeRepository, ModalidadeRepository>();
builder.Services.AddTransient<IAtletaRepository, AtletaRepository>();
builder.Services.AddTransient<IEventoRepository, EventoRepository>();

builder.Services.AddTransient<UsuarioService>();
builder.Services.AddTransient<AtleticaService>();
builder.Services.AddTransient<QRCodeService>();
builder.Services.AddTransient<ModalidadeService>();
builder.Services.AddTransient<AtletaService>();
builder.Services.AddTransient<EventoService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AtletiGo");
});

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();