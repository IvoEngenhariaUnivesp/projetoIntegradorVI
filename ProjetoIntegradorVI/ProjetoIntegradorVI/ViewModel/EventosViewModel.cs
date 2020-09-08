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

        public EventosViewModel()
        {
            ResultCommand = new Command(PushBackLogin);
        }

        public void PushBackLogin()
        {
            App.Current.MainPage.Navigation.RemovePage(new View.Login());
        }
    }
}
