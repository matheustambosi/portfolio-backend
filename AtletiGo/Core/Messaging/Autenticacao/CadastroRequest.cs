using AtletiGo.Core.Exceptions;
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
            if (string.IsNullOrWhiteSpace(Nome))
                throw new AtletiGoException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(Email))
                throw new AtletiGoException("Email é obrigatório.");

            if (string.IsNullOrWhiteSpace(Senha))
                throw new AtletiGoException("Senha é obrigatório.");

            if (string.IsNullOrWhiteSpace(RepeticaoSenha))
                throw new AtletiGoException("Repetição senha é obrigatório.");

            if (Senha != RepeticaoSenha)
                throw new AtletiGoException("As senhas não são iguais.");
        }
    }
}
