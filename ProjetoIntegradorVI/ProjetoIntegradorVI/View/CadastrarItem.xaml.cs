using ProjetoIntegradorVI.Domain.Model;
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
    public partial class CadastrarItem : ContentPage
    {
        CadItemViewModel cadItem = null;
        public CadastrarItem(Usuario usuario)
        {
            InitializeComponent();
            BindingContext = new CadItemViewModel(usuario);
            cadItem = new CadItemViewModel(usuario);
        }

        private void gramas_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / 100);
            gramas.Value = newStep * 100;
            lblText.Text = gramas.Value.ToString();
            lblText.TranslateTo(gramas.Value * ((gramas.Width - 40) / gramas.Maximum), 0, 100);

            if (gramas != null)
            {
                cadItem.getGramas = lblText.Text;
            }
        }

    }
}