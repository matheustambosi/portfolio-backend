using System;

namespace AtletiGo.Core.Messaging.RawQueryResult
{
    public class GetAllAtletasRawQueryResult
    {
        public Guid CodigoAtleta { get; set; }
        public string NomeAtleta { get; set; }
        public string Modalidade { get; set; }
    }
}
