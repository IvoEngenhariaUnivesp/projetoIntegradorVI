using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProjetoIntegradorVI.ViewModel
{
    public class EventoDetalheViewModel
    {
        private readonly FirebaseConfig<Evento> _firebaseConfig;
        public Command backCommand { get; set; }//btn voltar no topo
        public Command infoCommand { get; set; }//btn de informações no topo a direita
        public Command eventoCommand { get; set; }//btn no rodapé para o evento
        public Command itensCommand { get; set; }//btn no rodapé para ver os itens
        public Command conviteCommand { get; set; }//btn no rodapé para ver os convites
        private Usuario _usuarioLogado { get; set; }

        public EventoDetalheViewModel(Usuario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            _firebaseConfig = new FirebaseConfig<Evento>();



        }



    }
}
