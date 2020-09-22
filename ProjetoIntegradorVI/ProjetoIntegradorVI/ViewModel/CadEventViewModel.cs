using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProjetoIntegradorVI.ViewModel
{
    public class CadEventViewModel
    {
        public Command ResultadoCommand { get; set; }
        public FirebaseConfig<Evento> _eventoFirebase;
        public string nome { get; set; }
        public string descricao { get; set; }
        public TimeSpan horaInicio { get; set; }
        public DateTime dataInicio { get; set; } = DateTime.Now;
        public DateTime dataTermino { get; set; } = DateTime.Now;
        public TimeSpan horaTermino { get; set; }
        public string logradouro { get; set; }
        public string bairro { get; set; }
        public string estado { get; set; }
        public string cidade { get; set; }
        public string cep { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        private Usuario _usuarioLogado { get; set; }
        
        public CadEventViewModel(Usuario usuarioLogado)
        {
            _usuarioLogado = usuarioLogado;
            ResultadoCommand = new Command(CadastrarEvento);
        }

        public async void CadastrarEvento()
        {
            _eventoFirebase = new FirebaseConfig<Evento>();
            Evento evento = new Evento();
            evento.UsuarioCriadorID = _usuarioLogado.ID.Value;
            evento.Nome = nome;
            evento.Descricao = descricao;
            evento.ChaveEvento = new Random().Next().ToString();
            evento.DataInicio = dataInicio.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            evento.HoraInicio = horaInicio.ToString(@"hh\:mm");
            evento.DataTermino = dataTermino.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            evento.HoraTermino = horaTermino.ToString(@"hh\:mm");
            evento.Descricao = descricao == (null) ? "" : descricao;
            evento.Logradouro = logradouro == (null) ? "" : logradouro;
            evento.Bairro = bairro == (null) ? "" : bairro;
            evento.Estado = estado == (null) ? "" : estado;
            evento.Cidade = cidade == (null) ? "" : cidade;
            evento.CEP = cep == (null) ? "" : cep;
            evento.Numero = numero == (null) ? "" : numero;
            evento.Complemento = complemento == (null) ? "" : complemento;
            evento.Latitude = "";
            evento.Longitude = "";

            var cadEventoSucces = await _eventoFirebase.InsertEventoAsync(evento, _usuarioLogado);
            if (cadEventoSucces != null)
            {
                await App.Current.MainPage.DisplayAlert("Cadastro", "Sucesso no Cadastro!", "Ok");

                App.Current.MainPage = new NavigationPage(new View.Eventos(_usuarioLogado));

                await App.Current.MainPage.Navigation.PushModalAsync(new View.TabbedPageEventoDetalhe(_usuarioLogado, cadEventoSucces.ID.Value));
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Cadastro", "Houve um erro no cadastro. É possível que o este e-mail já exista.", "Voltar");
            }
        }
    }
}
