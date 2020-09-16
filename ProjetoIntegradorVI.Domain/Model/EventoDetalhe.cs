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
        public string ConvitesAceitos { get; set; }
        public string ConvitesPendentes { get; set; }
        public string ConvitesRecusados { get; set; }
        public string QuantidadeDiasRestantes { get; set; }
    }
}
