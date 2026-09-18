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
using Microsoft.Extensions.Configuration;
using AtletiGo.Core.Services.Autenticacao;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = builder.Configuration["CoreAuth:JwtKey"];
var issuer = builder.Configuration["CoreAuth:Issuer"];
var audience = builder.Configuration["CoreAuth:Audience"] ?? "atletigo";
if (Encoding.UTF8.GetByteCount(jwtKey ?? "") < 32 || string.IsNullOrWhiteSpace(issuer))
    throw new InvalidOperationException("Configure CoreAuth:JwtKey and CoreAuth:Issuer using the Core JWT settings.");
var key = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddHttpClient<CoreAuthClient>(client =>
{
    var url = builder.Configuration["CoreAuth:BaseUrl"] ?? throw new InvalidOperationException("CoreAuth:BaseUrl is required.");
    var uri = new Uri(url.TrimEnd('/') + "/");
    if (uri.Scheme != "https" && !(builder.Environment.IsDevelopment() && uri.IsLoopback))
        throw new InvalidOperationException("CoreAuth:BaseUrl must use HTTPS.");
    var apiKey = builder.Configuration["CoreAuth:ApiKey"];
    if (string.IsNullOrEmpty(apiKey) || apiKey.Length < 32) throw new InvalidOperationException("CoreAuth:ApiKey is required (at least 32 characters).");
    client.BaseAddress = uri;
    client.Timeout = TimeSpan.FromSeconds(20);
    client.DefaultRequestHeaders.Add("X-Project-Origin", "atletigo");
    client.DefaultRequestHeaders.Add("X-Project-Key", apiKey);
});
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser().RequireClaim("origin", "atletigo").RequireClaim("codigo_operador").Build();
});

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.MapInboundClaims = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateLifetime = true,
            ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha256 },
            NameClaimType = "name",
            RoleClaimType = "role",
            ClockSkew = TimeSpan.FromSeconds(30)
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
