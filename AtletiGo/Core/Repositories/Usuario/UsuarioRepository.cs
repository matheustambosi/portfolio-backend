using AtletiGo.Core.Services.Autenticacao;
using AtletiGo.Core.Messaging.Autenticacao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtletiGo.Core.Repositories.Usuario;

// Compatibility adapter for domain services. No connection to the Atletigo user table.
public sealed class UsuarioRepository(CoreAuthClient client) : IUsuarioRepository
{
    private static void OnlyUsers<T>()
    {
        if (typeof(T) != typeof(Entities.Usuario)) throw new NotSupportedException("Only central users are supported.");
    }
    public T GetById<T>(Guid codigo) where T : class
    {
        OnlyUsers<T>();
        return (T)(object)client.Get(codigo).ToUsuario();
    }
    public IEnumerable<T> GetAll<T>() where T : class => GetAll<T>(null);
    public IEnumerable<T> GetAll<T>(object obj = null) where T : class
    {
        OnlyUsers<T>();
        IEnumerable<Entities.Usuario> users = client.List().Select(u => u.ToUsuario());
        if (obj != null)
            foreach (var filter in obj.GetType().GetProperties())
            {
                var property = typeof(Entities.Usuario).GetProperty(filter.Name)
                    ?? throw new NotSupportedException("Unsupported user filter.");
                var value = filter.GetValue(obj);
                users = users.Where(u => Equals(property.GetValue(u), value));
            }
        return users.Cast<T>().ToList();
    }
    public void Update<T>(T entity)
    {
        OnlyUsers<T>();
        client.Update((Entities.Usuario)(object)entity);
    }
    public Entities.Usuario Register(string name, string email, string password) => client.Register(name, email, password);
    public LoginResponse Login(string email, string password) => client.Login(email, password);
    public void Insert<T>(T entity) => throw new NotSupportedException("Use shared registration.");
    public void Delete<T>(T entity) => throw new NotSupportedException("Unlink project scopes instead of deleting the global identity.");
    public IEnumerable<T> Query<T>(string sql, object obj = null) where T : class => throw new NotSupportedException("Users live in Core; SQL is not supported.");
}
