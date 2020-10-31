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
        public EventoLocalizacao()
        {
            InitializeComponent();
        }

        private void AbreMapa_Clicked(object sender, System.EventArgs e)
        {
            CrossExternalMaps.Current.NavigateTo("Teste", "Rua Francisco Mascarenhas, 205B", "São Paulo", "São Paulo", "02808030", "BR", "55");
        }
    }
}