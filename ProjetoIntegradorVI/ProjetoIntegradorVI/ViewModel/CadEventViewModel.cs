using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ProjetoIntegradorVI.ViewModel
{
    public class CadEventViewModel
    {
        public Command ResultadoCommand { get; set; }
        public readonly FirebaseConfig<Evento> _eventoFirebase;
        public string nome { get; set; }
        public string descricao { get; set; }
        public TimePicker horaInicio { get; set; }
        public string dataInicio { get; set; }
        public string dataTermino { get; set; }
        public TimePicker horaTermino { get; set; }
        public string logradouro { get; set; }
        public string bairro { get; set; }
        public string estado { get; set; }
        public string cidade { get; set; }
        public string cep { get; set; }
        public int numero { get; set; }
        public string complemento { get; set; }

        public CadEventViewModel()
        {
            _eventoFirebase = new FirebaseConfig<Evento>();
            ResultadoCommand = new Command(CadastrarEvento);
        }

        public async void CadastrarEvento()
        {
            Evento evento = new Evento();
            evento.Nome = nome;
            evento.Descricao = descricao;
            evento.DataInicio = dataInicio;
            evento.HoraInicio = horaInicio.ToString();
            evento.DataTermino = dataTermino;
            evento.HoraTermino = horaTermino.ToString();
            evento.Descricao = descricao;
            evento.Logradouro = logradouro;
            evento.Bairro = bairro;
            evento.Estado = estado;
            evento.Cidade = cidade;
            evento.CEP = cep;
            evento.Numero = numero;
            evento.Complemento = complemento;

            var cadEventoSucces = await _eventoFirebase.InsertEventoAsync(evento);
            if (cadEventoSucces != null)
            {
                await App.Current.MainPage.DisplayAlert("Cadastro", "Sucesso no Cadastro!", "Ok");

                App.Current.MainPage = new NavigationPage(new View.EventoDetalhe());
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Cadastro", "Houve um erro no cadastro. É possível que o este e-mail já exista.", "Voltar");
            }
        }
    }
}
