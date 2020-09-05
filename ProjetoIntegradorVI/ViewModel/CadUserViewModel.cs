using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
//using ProjetoIntegradorVI.Database;

namespace ProjetoIntegradorVI.ViewModel
{
    public class CadUserViewModel
    {
        public Command ResultadoCommand { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string cell { get; set; }
        public string senha { get; set; }

        public CadUserViewModel()
        {
            
        }

        public void Logar()
        {

        }

    }
}
