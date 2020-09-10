using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.Database;


namespace ProjetoIntegradorVI.ViewModel
{
    public class EventosViewModel
    {
        public Command ResultCommand { get; set; }
        public Command AdicionarEvento { get; set; }

        public EventosViewModel()
        {
            ResultCommand = new Command(PushBackLogin);
            AdicionarEvento = new Command(AddEvent);
        }

        public void PushBackLogin()
        {
            App.Current.MainPage.Navigation.RemovePage(new View.Login());
        }

        public void AddEvent()
        {
            App.Current.MainPage.Navigation.PushModalAsync(new View.CadastrarEvento());
            //App.Current.MainPage = new NavigationPage(new View.CadastrarEvento());
        }
    }
}
