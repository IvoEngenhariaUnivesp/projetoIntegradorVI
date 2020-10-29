using ProjetoIntegradorVI.Domain.Model.Common;
using ProjetoIntegradorVI.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetoIntegradorVI.Domain.Model
{
    public class EventoUsuarioDetalhe : EventoUsuario
    {
        public string NomeUsuario { get; set; }
    }
}
