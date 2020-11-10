using Plugin.ExternalMaps;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventoLocalizacao : ContentPage
    {
        public EventoDetalhe _evento { get; set; }
        public EventoLocalizacao(EventoDetalhe evento)
        {
            InitializeComponent();

            _evento = evento;

            lblEndereco.Text = _evento.LogradouroEvento + ", " + _evento.NumeroEvento;
            lblCidadeEstado.Text = _evento.CidadeEvento + " - " + _evento.EstadoEvento;
            lblCEP.Text = _evento.CEPEvento;
        }

        private void AbreMapa_Clicked(object sender, System.EventArgs e)
        {
            CrossExternalMaps.Current.NavigateTo("Localização", _evento.LogradouroEvento+", "+_evento.NumeroEvento, _evento.CidadeEvento, _evento.EstadoEvento, _evento.CEPEvento, "BR", "55");
        }
    }
}