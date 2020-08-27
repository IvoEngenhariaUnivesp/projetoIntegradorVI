using ProjetoIntegradorVI.Model.Common;
using System;

namespace ProjetoIntegradorVI.Model
{
    public class Evento : EntidadeBase
    {
        public long UsuarioCriadorID { get; set; }
        public string ChaveEvento { get; set; }
        public string Nome { get; set; }
        public DateTime DataHoraInicio { get; set; }
        public DateTime DataHoraTermino { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public long CEP { get; set; }
        public int Numero { get; set; }
        public string Complemento { get; set; }
    }
}
