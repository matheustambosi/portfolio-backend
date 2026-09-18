using System;

namespace AtletiGo.Core.Messaging.RawQueryResult
{
    public class GetAllAtletasRawQueryResult
    {
        public Guid CodigoAtleta { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid CodigoUsuario { get; set; }
        public string NomeAtleta { get; set; }
        public string Modalidade { get; set; }
    }
}
