using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using AtletiGo.Core.Exceptions;
using AtletiGo.Core.Messaging.Autenticacao;
using AtletiGo.Core.Utils.Enums;

namespace AtletiGo.Core.Services.Autenticacao;

public sealed class CoreUserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
    public Dictionary<string, string> Scopes { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Entities.Usuario ToUsuario() => new()
    {
        Codigo = Id, Nome = Name, Email = Email,
        CodigoAtletica = Scopes.TryGetValue("codigo_atletica", out var id) && Guid.TryParse(id, out var association) ? association : null,
        TipoUsuario = Scopes.TryGetValue("role", out var role) && Enum.TryParse<TipoUsuario>(role, out var type) ? type : TipoUsuario.Nenhum,
        DtCriacao = CreatedAt, DtAlteracao = UpdatedAt
    };
}

public sealed class CoreAuthClient(HttpClient client)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private T Send<T>(HttpMethod method, string path, object body = null)
    {
        using var request = new HttpRequestMessage(method, path);
        if (body != null) request.Content = JsonContent.Create(body);
        using var response = client.Send(request);
        if (!response.IsSuccessStatusCode)
            throw new AtletiGoException(response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "Credenciais invalidas ou integracao com o Core nao autorizada.",
                HttpStatusCode.NotFound => "Usuario nao encontrado no Core.",
                HttpStatusCode.Conflict => "Conta ja existente ou alteracao de email exige verificacao no Core.",
                HttpStatusCode.BadRequest => "Dados invalidos. Confira nome, email, senha (minimo 8 caracteres) e permissoes.",
                _ => "Nao foi possivel concluir a operacao no Core."
            });
        return JsonSerializer.Deserialize<T>(response.Content.ReadAsStream(), JsonOptions)
            ?? throw new AtletiGoException("Resposta invalida do Core.");
    }
    public Entities.Usuario Register(string name, string email, string password) =>
        Send<CoreUserResponse>(HttpMethod.Post, "auth/shared/register", new { origin = "atletigo", name, email, password }).ToUsuario();
    public LoginResponse Login(string email, string password)
    {
        var response = Send<CoreUserResponse>(HttpMethod.Post, "auth/shared/login", new { origin = "atletigo", email, password });
        return new LoginResponse(response.ToUsuario(), response.Token);
    }
    public CoreUserResponse Get(Guid id) => Send<CoreUserResponse>(HttpMethod.Get, $"auth/shared/users/{id}");
    public List<CoreUserResponse> List() => Send<List<CoreUserResponse>>(HttpMethod.Get, "auth/shared/users");
    public void Update(Entities.Usuario user) => Send<CoreUserResponse>(HttpMethod.Put, $"auth/shared/users/{user.Codigo}/scopes", new
    {
        name = user.Nome, email = user.Email,
        scopes = new Dictionary<string, string>
        {
            ["role"] = user.TipoUsuario.ToString(),
            ["codigo_atletica"] = user.CodigoAtletica?.ToString() ?? ""
        }
    });
}
