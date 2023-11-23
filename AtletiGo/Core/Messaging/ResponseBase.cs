using AtletiGo.Core.Exceptions;
using System;

namespace AtletiGo.Core.Messaging
{
    [Serializable]
    public class ResponseBase : ResponseBase<ResponseBase>
    {

    }

    [Serializable]
    public class ResponseBase<TResponse> where TResponse : ResponseBase<TResponse>
    {
        public ResponseBase()
        {
            Sucesso = true;
            Mensagem = string.Empty;
        }

        public ResponseBase(bool sucesso, string mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
        }

        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }

        public static TResponse ErroGenerico()
        {
            var response = Activator.CreateInstance<TResponse>();
            response.SetErroGenerico();
            return response;
        }

        public static TResponse ErroAtletiGo(AtletiGoException spbEx)
        {
            var response = Activator.CreateInstance<TResponse>();
            response.SetErroAtletiGo(spbEx);
            return response;
        }

        public void SetErroGenerico()
        {
            Sucesso = false;
            Mensagem = "Ocorreu um erro na requisição.";
        }

        public void SetErroAtletiGo(AtletiGoException ex)
        {
            Sucesso = false;
            Mensagem = ex.Message;
        }
    }
}
