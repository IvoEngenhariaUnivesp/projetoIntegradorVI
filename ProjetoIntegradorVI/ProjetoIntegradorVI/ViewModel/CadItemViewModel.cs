using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.Domain.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace ProjetoIntegradorVI.ViewModel
{
    public class CadItemViewModel : INotifyPropertyChanged
    {
        public FirebaseConfig<EventoItem> _eventoItemFirebase;

        public event PropertyChangedEventHandler PropertyChanged;
        
        public long _evento { get; set; }
        public Command getGramasCommand { get; set; }
        public Command ResultadoCommand { get; set; }
        public string getGramas { get; set; }
        public string getNome { get; set; }
        public string quantidadeTotal { get; set; }
        public string getTipo { get; set; }
        public string getTipoUnidade { get; set; }
        private Usuario _usuarioLogado { get; set; }

        public CadItemViewModel(Usuario usuario, long eventoId)
        {
            _usuarioLogado = usuario;
            _evento = eventoId;
            ResultadoCommand = new Command(CadastrarItemEvento);
        }

        public async void CadastrarItemEvento()
        {
            _eventoItemFirebase = new FirebaseConfig<EventoItem>();
            EventoItem eventoItem = new EventoItem();
            eventoItem.EventoID = _evento;
            eventoItem.Nome = getNome;
            eventoItem.QuantidadeDesejada = quantidadeTotal;
            eventoItem.Tipo = (TipoItemEnum)Enum.Parse(typeof(TipoItemEnum), getTipo);
            eventoItem.TipoUnidade = getTipoUnidade;
            var obj = Enum.GetNames(typeof(TipoItemEnum));

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(getGramas));
            //eventoItem.QuantidadeDesejada = getGramas;
            var cadItemEvento = await _eventoItemFirebase.InsertEventoItemAsync(eventoItem, _usuarioLogado);
            if(cadItemEvento != null)
            {
                await App.Current.MainPage.DisplayAlert("Cadastro", "Sucesso no Cadastro!", "Ok");

                //App.Current.MainPage = new NavigationPage(new View.Eventos(_usuarioLogado));
                await App.Current.MainPage.Navigation.PopModalAsync();

                //await App.Current.MainPage.Navigation.PushModalAsync(new View.TabbedPageEventoDetalhe(_usuarioLogado, cadEventoSucces.ID.Value));
            }
            else
                await App.Current.MainPage.DisplayAlert("ERRO!", "Erro no Cadastro!", "Ok");
        }


    }
}
