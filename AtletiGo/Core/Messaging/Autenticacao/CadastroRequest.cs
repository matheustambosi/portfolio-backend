using System;

namespace AtletiGo.Core.Messaging.Autenticacao
{
    public class CadastroRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string RepeticaoSenha { get; set; }

        public void Validar()
        {

        }
    }
}
