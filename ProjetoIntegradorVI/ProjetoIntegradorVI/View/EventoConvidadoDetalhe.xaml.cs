using Plugin.ExternalMaps;
using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.ViewModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventoConvidadoDetalhe : ContentPage
    {
        public EventoConvidadoDetalhe(long usuarioMembroID)
        {
            InitializeComponent();

            FirebaseConfig<EventoItemUsuario> _client = new FirebaseConfig<EventoItemUsuario>();

            Domain.Model.EventoConvidadoDetalhe eventoConvidadoDetalhe = new Domain.Model.EventoConvidadoDetalhe();

            Task.Run(async () => {
                eventoConvidadoDetalhe = await _client.GetEventoConvidadoDetalheByUsuarioIDAsync(usuarioMembroID);

                lblNome.Text = "Nome:" + eventoConvidadoDetalhe.Nome;
                lblItensQueVaiLevar.Text = eventoConvidadoDetalhe.ItemQuantidade;
            }).Wait();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            App.Current.MainPage.Navigation.PopModalAsync(true);
        }
    }
}