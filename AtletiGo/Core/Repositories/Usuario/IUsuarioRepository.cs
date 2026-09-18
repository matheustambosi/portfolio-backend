namespace AtletiGo.Core.Repositories.Usuario
{
    public interface IUsuarioRepository : IRepositoryBase
    {
        Entities.Usuario Register(string name, string email, string password);
        Messaging.Autenticacao.LoginResponse Login(string email, string password);
    }
}
