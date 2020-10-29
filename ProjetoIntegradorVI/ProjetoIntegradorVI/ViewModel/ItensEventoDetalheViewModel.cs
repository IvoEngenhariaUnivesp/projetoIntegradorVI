using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProjetoIntegradorVI.ViewModel
{
    public class ItensEventoDetalheViewModel
    {
        private readonly FirebaseConfig<Evento> _firebaseClient;
        public ICommand AcessEventoCommand { get; set; }
        private Usuario _usuarioLogado { get; set; }
        private long _eventoID { get; set; }
        public string NomeEvento { get; set; }


        public ItensEventoDetalheViewModel(Usuario usuarioLogado, long eventoID)
        {
            //Deixa o usuário logado em memória
            _usuarioLogado = usuarioLogado;
            _firebaseClient = new FirebaseConfig<Evento>();
            _eventoID = eventoID;
            //AcessEventoCommand = new Command(async (object obj) => await AccessEventoDetalheCommand(obj));

        }
    }
}
