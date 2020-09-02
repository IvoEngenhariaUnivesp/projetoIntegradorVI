using ProjetoIntegradorVI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ProjetoIntegradorVI.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        public void Teste()
        {
            new NavigationPage(new CadastroUsuario());
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Teste();
        }
    }
}