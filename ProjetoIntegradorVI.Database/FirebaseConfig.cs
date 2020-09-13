using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ProjetoIntegradorVI.Domain.Model.Common;
using ProjetoIntegradorVI.Domain.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq.Expressions;
using ProjetoIntegradorVI.Domain.Model.Enums;

namespace ProjetoIntegradorVI.Database
{
    public class FirebaseConfig<T> where T : EntidadeBase
    {
        private FirebaseClient _client;

        private void InstaciaClient()
        {
            _client = new Firebase.Database.FirebaseClient(
              "https://projetointegradorvi-4n1.firebaseio.com/",
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult("em8oKbMoEJvVxipNAMW9ZPItgWHxO3P9iW04VLGf")
              });

        }

        #region Métodos Genéricos
        #endregion Métodos Genéricos

        #region Métodos Child Usuário
        public async Task<Usuario> InsertUsuarioAsync(Usuario objeto)
        {
            // Abre a conexão com o banco
            InstaciaClient();

            //Verificando se tem o mesmo email
            var usuarioRetorno =
                await _client
                    .Child("Usuarios")
                    .OrderBy("Email")
                    .EqualTo(objeto.Email)
                    .OnceSingleAsync<Dictionary<long, Usuario>>();

            // Caso retorne null, quer dizer que não existe usuário com o mesmo email
            if (usuarioRetorno.Values.Count < 1)
            {
                // Atribui ao objeto o ultimo ID do banco + 1
                try
                {
                    // Liberado para cadastrar
                    // Pega ultimo registro do banco em forma de lista, onde o primeiro objeto podeser nulo.
                    var ultimoRegistro = await _client
                        .Child("Usuarios")
                        .OrderByKey()
                        .LimitToLast(1)
                        .OnceSingleAsync<List<Usuario>>();

                    objeto.ID = ultimoRegistro.Last().ID + 1;
                }
                catch (Exception ex)
                {
                    objeto.ID = 0;
                }

                try
                {
                    // Insere o objeto no Chil Usuarios/ID
                    await _client.Child("Usuarios").Child(objeto.ID.ToString()).PutAsync(objeto);
                }
                catch (Exception)
                {
                    return default(Usuario);
                }
            }
            else
            {
                return default(Usuario);
            }

            return objeto;
        }

        public async Task<Usuario> GetUsuarioByEmailAsync(string email)
        {
            // Abre a conexão com o banco
            InstaciaClient();

            try
            {
                // Busca o objeto com o mesmo email no banco
                // Retorna um dictionary com <ID, Usuario>↨
                var usuarioRetorno =
                    await _client
                        .Child("Usuarios")
                        .OrderBy("Email")
                        .EqualTo(email)
                        .OnceSingleAsync<Dictionary<long, Usuario>>();

                // Retorna o primeiro objeto(sempre vai ser um, se encontrar)
                return usuarioRetorno.Values.First();
            }
            catch (Exception)
            {
                // Se tiver não existir, retorna null
                return default(Usuario);
            }
        }
        #endregion Métodos Child Usuário

        #region Métodos Child Eventos

        //public async Task<Evento> InsertEventoAsync(Evento objeto)
        //{
        //    // Abre a conexão com o banco
        //    InstaciaClient();

        //    // Atribui ao objeto o ultimo ID do banco + 1
        //    try
        //    {
        //        // Liberado para cadastrar
        //        // Pega ultimo registro do banco em forma de lista, onde o primeiro objeto podeser nulo.
        //        var ultimoRegistro = await _client
        //            .Child("Eventos")
        //            .OrderByKey()
        //            .LimitToLast(1)
        //            .OnceSingleAsync<List<Evento>>();

        //        objeto.ID = ultimoRegistro.Last().ID + 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        objeto.ID = 0;
        //    }

        //    try
        //    {
        //        // Insere o objeto no Chil Usuarios/ID
        //        await _client.Child("Eventos").Child(objeto.ID.ToString()).PutAsync(objeto);
        //    }
        //    catch (Exception)
        //    {
        //        return default(Evento);
        //    }

        //    return objeto;
        //}
        public async Task<Evento> InsertEventoAsync(Evento objeto, Usuario usuario = null)
        {
            // Abre a conexão com o banco
            InstaciaClient();

            // Atribui ao objeto o ultimo ID do banco + 1
            try
            {
                // Liberado para cadastrar
                // Pega ultimo registro do banco em forma de lista, onde o primeiro objeto podeser nulo.
                var ultimoRegistro = await _client
                    .Child("Eventos")
                    .OrderByKey()
                    .LimitToLast(1)
                    .OnceSingleAsync<List<Evento>>();

                objeto.ID = ultimoRegistro.Last().ID + 1;
            }
            catch (Exception ex)
            {
                objeto.ID = 0;
            }

            try
            {
                var pegaUsuario = await _client.Child("EventoUsuario").OrderByKey().LimitToLast(1).OnceSingleAsync<EventoUsuario>();
                // Insere o objeto no Chil Usuarios/ID
                await _client.Child("Eventos").Child(objeto.ID.ToString()).PutAsync(objeto);

                if (usuario != null)
                {
                    await _client.Child("EventoUsuario").Child(objeto.ID.ToString()).PutAsync(new EventoUsuario { EventoID = objeto.ID.Value, ID = objeto.ID, StatusConvite = StatusConviteEnum.Aceito, UsuarioMembroID = usuario.ID.Value });
                }
            }
            catch (Exception)
            {
                return default(Evento);
            }

            return objeto;
        }

        public async Task<List<Evento>> GetAllEventosByUsuarioIDAsync(long usuarioID)
        {
            // Abre a conexão com o banco
            InstaciaClient();

            List<Evento> eventosRetorno = new List<Evento>();

            try
            {
                // Busca o objeto com o mesmo email no banco
                // Retorna um dictionary com <ID, Usuario>
                var eventosByUsuarioIDRetorno =
                    await _client
                        .Child("EventoUsuario")
                        .OrderBy("UsuarioMembroID")
                        .EqualTo(usuarioID)
                        .OnceSingleAsync<Dictionary<long, EventoUsuario>>();

                foreach (EventoUsuario eventoUsuario in eventosByUsuarioIDRetorno.Values)
                {
                    if (eventoUsuario.StatusConvite == StatusConviteEnum.Recusado)
                        continue;

                    var evento = await GetEventoByEventoIDAsync(eventoUsuario.EventoID);

                    if (evento != null) //&& evento.DataHoraTermino < DateTime.Now)
                        eventosRetorno.Add(evento);
                }
            }
            catch (Exception ex)
            {

            }
            return eventosRetorno;
        }

        public async Task<Evento> GetEventoByEventoIDAsync(long eventoID)
        {
            // Abre a conexão com o banco
            InstaciaClient();

            try
            {
                // Busca o objeto com o mesmo email no banco
                // Retorna um dictionary com <ID, Evento>
                var eventoRetorno =
                    await _client
                        .Child("Eventos")
                        .OrderBy("ID")
                        .EqualTo(eventoID)
                        .OnceSingleAsync<Dictionary<long, Evento>>();

                // Retorna o primeiro objeto(sempre vai ser um, se encontrar)
                return eventoRetorno.Values.First();
            }
            catch (Exception ex)
            {
                // Se tiver não existir, retorna null
                return default(Evento);
            }
        }

        #endregion Métodos Child Eventos

    }
}
