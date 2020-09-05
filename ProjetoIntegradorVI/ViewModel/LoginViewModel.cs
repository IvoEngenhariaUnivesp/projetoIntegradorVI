using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProjetoIntegradorVI.ViewModel
{
    public class LoginViewModel
    {
        public Command ResultCommand { get; set; }

        public LoginViewModel()
        {
            ResultCommand = new Command(CadastroUser);
        }

        public void CadastroUser()
        {


            //App.Current.MainPage = new NavigationPage(new View.CadastroUsuario());
            App.Current.MainPage.Navigation.PushModalAsync(new View.CadastroUsuario());
        }
    }
}
