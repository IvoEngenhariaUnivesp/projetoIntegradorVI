using ProjetoIntegradorVI.Domain.Model.Common;
using System;

namespace ProjetoIntegradorVI.Domain.Model
{
    public class Evento : EntidadeBase
    {
        public long UsuarioCriadorID { get; set; }
        public string ChaveEvento { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string DataInicio { get; set; }
        public string HoraInicio { get; set; }
        public string DataTermino { get; set; }
        public string HoraTermino { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
