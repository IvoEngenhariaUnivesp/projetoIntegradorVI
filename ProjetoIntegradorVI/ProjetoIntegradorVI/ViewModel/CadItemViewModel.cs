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
        public FirebaseConfig<EventoItemUsuario> _eventoItemUsuarioFirebase;

        public event PropertyChangedEventHandler PropertyChanged;

        public long _evento { get; set; }
        public long _eventoItemID { get; set; }
        public Command getGramasCommand { get; set; }
        public Command ResultadoCommand { get; set; }
        public Command ResultadoConvidadoCommand { get; set; }
        public string getGramas { get; set; }
        public string getNome { get; set; }
        public string quantidadeTotal { get; set; }
        public string getTipo { get; set; }
        public string getTipoUnidade { get; set; }
        private Usuario _usuarioLogado { get; set; }

        public CadItemViewModel(Usuario usuario, long eventoId, long eventoItemID)
        {
            _usuarioLogado = usuario;
            _evento = eventoId;
            _eventoItemID = eventoItemID;
            ResultadoCommand = new Command(CadastrarItemEvento);
            ResultadoConvidadoCommand = new Command(CadastrarItemEventoConvidado);
        }

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

            var cadItemEvento = await _eventoItemFirebase.InsertEventoItemAsync(eventoItem, _usuarioLogado);
            if(cadItemEvento != null)
            {
                await App.Current.MainPage.DisplayAlert("Cadastro", "Sucesso no Cadastro!", "Ok");
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
            else
                await App.Current.MainPage.DisplayAlert("ERRO!", "Erro no Cadastro!", "Ok");
        }

        public async void CadastrarItemEventoConvidado()
        {
            _eventoItemUsuarioFirebase = new FirebaseConfig<EventoItemUsuario>();
            EventoItemUsuario eventoItemUsuario = new EventoItemUsuario();
            eventoItemUsuario.EventoID = _evento;
            eventoItemUsuario.EventoItemID = _eventoItemID;
            eventoItemUsuario.Quantidade = quantidadeTotal;
            eventoItemUsuario.UsuarioID = _usuarioLogado.ID.Value;

            var cadItemEvento = await _eventoItemUsuarioFirebase.InsertEventoItemUsuarioAsync(eventoItemUsuario, _usuarioLogado);
            if (cadItemEvento != null)
            {
                await App.Current.MainPage.DisplayAlert("Cadastro", "Sucesso no Cadastro!", "Ok");
                await App.Current.MainPage.Navigation.PopModalAsync();
            }
            else
                await App.Current.MainPage.DisplayAlert("ERRO!", "Erro no Cadastro!", "Ok");
        }


    }
}
