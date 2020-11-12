using ProjetoIntegradorVI.Domain.Model;
using ProjetoIntegradorVI.Domain.Model.Enums;
using ProjetoIntegradorVI.ViewModel;
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
    public partial class CadastrarItemConvidado : ContentPage
    {
        CadItemViewModel cadItem = null;
        public CadastrarItemConvidado(Usuario usuario, long eventoId, long eventoItemID)
        {
            InitializeComponent();
            BindingContext = new CadItemViewModel(usuario, eventoId, eventoItemID);
            cadItem = new CadItemViewModel(usuario, eventoId, eventoItemID);
        }
    }
}