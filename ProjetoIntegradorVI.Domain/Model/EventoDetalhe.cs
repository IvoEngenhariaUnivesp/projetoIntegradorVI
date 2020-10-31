using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoIntegradorVI.Domain.Model
{
    public class EventoDetalhe
    {
        public long EventoID { get; set; }
        public long UsuarioID { get; set; }
        public string NomeEvento { get; set; }
        public string DescricaoEvento { get; set; }
        public string LogradouroEvento { get; set; }
        public string BairroEvento { get; set; }
        public string EstadoEvento { get; set; }
        public string CidadeEvento { get; set; }
        public string CEPEvento { get; set; }
        public string NumeroEvento { get; set; }
        public string ComplementoEvento { get; set; }
        public string DataInicio { get; set; }
        public string DataTermino { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public int DiasRestantes { get; set; }
        public int ConvitesAceitos { get; set; }
        public int ConvitesPendentes { get; set; }
        public int ConvitesRecusados { get; set; }
    }
}
