using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace ProjetoIntegradorVI.ViewModel
{
    public class ItensEventoDetalheViewModel
    {
        private readonly FirebaseConfig<EventoItem> _firebaseClient;
        public ICommand AcessEventoCommand { get; set; }
        private Usuario _usuarioLogado { get; set; }
        private long _eventoID { get; set; }
        public string NomeEvento { get; set; }
        public string nome { get; set; }
        public string quantidadeTotal { get; set; }
        public string tipoUnidade { get; set; }


        public ItensEventoDetalheViewModel(Usuario usuarioLogado, long eventoID)
        {
            var teste = new EventoItem();
            List<EventoItem> items = new List<EventoItem>(); 
            //Deixa o usuário logado em memória
            _usuarioLogado = usuarioLogado;
            _firebaseClient = new FirebaseConfig<EventoItem>();
            _eventoID = eventoID;
            //AcessEventoCommand = new Command(async (object obj) => await AccessEventoDetalheCommand(obj));
            //Task.Run(async () =>
            //{
            //    items = await _firebaseClient.GetEventoItemAsync(_eventoID);
            //}).Wait();
            //foreach(var i in items)
            //{
            //    nome = i.Nome;
            //}
        }
    }
}
