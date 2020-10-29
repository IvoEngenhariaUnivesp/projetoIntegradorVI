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
using System.Globalization;

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
                catch (Exception)
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
                // Pega ultimo registro do banco em forma de lista, onde o primeiro objeto pode ser nulo.
                var ultimoRegistro = await _client
                    .Child("Eventos")
                    .OrderByKey()
                    .LimitToLast(1)
                    .OnceSingleAsync<Dictionary<object,Evento>>();

                objeto.ID = ultimoRegistro != (null) ? ultimoRegistro.Values.Last().ID + 1 : 0;
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.ResponseData.StartsWith("[{") || ex.ResponseData.StartsWith("[null"))
                {
                    var ultimoRegistro = await _client
                        .Child("Eventos")
                        .OrderByKey()
                        .LimitToLast(1)
                        .OnceSingleAsync<List<Evento>>();

                    objeto.ID = ultimoRegistro.Last().ID + 1;
                }
                else
                {
                    objeto.ID = 0;
                }
            }

            try
            {
                // Insere o objeto no Chil Usuarios/ID
                await _client.Child("Eventos").Child(objeto.ID.ToString()).PutAsync(objeto);

                var pegaEventoUsuario = await _client.Child("EventoUsuario").OrderByKey().LimitToLast(1).OnceSingleAsync<Dictionary<object, EventoUsuario>>();

                var eventoUsuarioID = pegaEventoUsuario == (null) ? 0 : pegaEventoUsuario.Values.Last().ID+1;

                if (usuario != null)
                {
                    await _client.Child("EventoUsuario").Child(eventoUsuarioID.ToString()).PutAsync(new EventoUsuario { EventoID = objeto.ID.Value, ID = eventoUsuarioID, StatusConvite = StatusConviteEnum.Aceito, UsuarioMembroID = usuario.ID.Value });
                }
            }
            catch (Firebase.Database.FirebaseException ex)
            {
                if (ex.ResponseData.StartsWith("[{") || ex.ResponseData.StartsWith("[null"))
                {
                    var pegaEventoUsuario = await _client.Child("EventoUsuario").OrderByKey().LimitToLast(1).OnceSingleAsync<List<EventoUsuario>>();
                    var eventoUsuarioID = pegaEventoUsuario == (null) ? 0 : pegaEventoUsuario.Last().ID + 1;
                    if (usuario != null)
                    {
                        await _client.Child("EventoUsuario").Child(eventoUsuarioID.ToString()).PutAsync(new EventoUsuario { EventoID = objeto.ID.Value, ID = eventoUsuarioID, StatusConvite = StatusConviteEnum.Aceito, UsuarioMembroID = usuario.ID.Value });
                    }
                }
                return objeto;
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
            catch (Exception)
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
            catch (Exception)
            {
                // Se tiver não existir, retorna null
                return default(Evento);
            }
        }

        public async Task<EventoDetalhe> GetEventoDetalheByEventoIDAsync(long eventoID)
        {
            // Abre a conexão com o banco
            InstaciaClient();

            EventoDetalhe eventoDetalhe = new EventoDetalhe();

            try
            {
                // Busca o evento com base no ID informado
                // Retorna um dictionary com <ID, Evento>
                var eventoRetorno =
                    await _client
                        .Child("Eventos")
                        .OrderBy("ID")
                        .EqualTo(eventoID)
                        .OnceSingleAsync<Dictionary<long, Evento>>();

                // Retorna o primeiro objeto(sempre vai ser um, se encontrar)
                var evento = eventoRetorno.Values.First();

                // Monta o objeto de retorno
                eventoDetalhe.EventoID = evento.ID.Value;
                eventoDetalhe.UsuarioID = evento.UsuarioCriadorID;
                eventoDetalhe.NomeEvento = evento.Nome;
                eventoDetalhe.DescricaoEvento = evento.Descricao;
                eventoDetalhe.DataInicio = evento.DataInicio;
                eventoDetalhe.DataTermino = evento.DataTermino;
                eventoDetalhe.HoraInicio = evento.HoraInicio;
                eventoDetalhe.HoraTermino = evento.HoraTermino;

                var dataEvento = DateTime.ParseExact(evento.DataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture).Day;
                var dataAtual = DateTime.Now.Day;
                eventoDetalhe.DiasRestantes = dataEvento - dataAtual;

                // Busca a lista de usuários vinculados ao evento
                // Retorna um dictionary com <ID, Usuario>
                var eventosByUsuarioIDRetorno =
                    await _client
                        .Child("EventoUsuario")
                        .OrderBy("EventoID")
                        .EqualTo(eventoID)
                        .OnceSingleAsync<Dictionary<long, EventoUsuario>>();

                // Monta a quantidade de convites aceitos, pendentes e recusados
                foreach (EventoUsuario eventoUsuario in eventosByUsuarioIDRetorno.Values)
                {
                    if (eventoUsuario.StatusConvite == StatusConviteEnum.Aceito)
                        eventoDetalhe.ConvitesAceitos++;

                    if (eventoUsuario.StatusConvite == StatusConviteEnum.Pendente)
                        eventoDetalhe.ConvitesPendentes++;

                    if (eventoUsuario.StatusConvite == StatusConviteEnum.Recusado)
                        eventoDetalhe.ConvitesRecusados++;
                }

            }
            catch (Exception)
            {
                // Se tiver não existir, retorna null
                return default(EventoDetalhe);
            }
            return eventoDetalhe;
        }

        public async Task<List<EventoUsuario>> GetConvitesPendentesByEventoIDAsync(long eventoID)
        {
            // Lista de Retorno dos Convites
            List<EventoUsuario> lstConvites = new List<EventoUsuario>();


            // Abre a conexão com o banco
            InstaciaClient();

            // Busca a lista de usuários vinculados ao evento
            // Retorna um dictionary com <ID, Usuario>
            var eventosByUsuarioIDRetorno =
                await _client
                    .Child("EventoUsuario")
                    .OrderBy("EventoID")
                    .EqualTo(eventoID)
                    .OnceSingleAsync<Dictionary<long, EventoUsuario>>();

            // Monta a quantidade de convites aceitos, pendentes e recusados
            foreach (EventoUsuario eventoUsuario in eventosByUsuarioIDRetorno.Values)
            {
                if (eventoUsuario.StatusConvite == StatusConviteEnum.Pendente)
                    lstConvites.Add(eventoUsuario);
            }

            return lstConvites;
        }

        #endregion Métodos Child Eventos

        #region Métodos Child Itens
        public async Task<EventoItem> InsertEventoItemAsync(EventoItem eventoItem, Usuario usuario = null)
        {
            InstaciaClient();
            try
            {
                var ultimoRegistro = await _client.Child("EventoItem").OrderByKey().LimitToLast(1).OnceSingleAsync<Dictionary<object, EventoItem>>();

                eventoItem.ID = ultimoRegistro != null ? ultimoRegistro.Values.Last().ID + 1 : 0;

                if (eventoItem.ID != null)
                    await _client.Child("EventoItem").Child(eventoItem.ID.ToString()).PutAsync(eventoItem);

                //if(usuario != null)
                //{
                //    await _client.Child("EventoItem").Child(usuario.ID.ToString()).PutAsync(new EventoItem { ID = eventoItem.ID, EventoID = eventoItem.ID.Value, Tipo = eventoItem.Tipo, Nome = eventoItem.Nome, TipoUnidade = eventoItem.TipoUnidade, QuantidadeDesejada = eventoItem.QuantidadeDesejada });
                //}

            }
            catch (Firebase.Database.FirebaseException ex)
            {

                if (ex.ResponseData.StartsWith("[{") || ex.ResponseData.StartsWith("[null"))
                {
                    var ultimoRegistro = await _client.Child("EventoItem").OrderByKey().LimitToLast(1).OnceSingleAsync<List<EventoItem>>();
                    //if(ultimoRegistro.Count > 0)
                    //{
                    //    if(ultimoRegistro[0] == null)
                    //    {
                    //        ultimoRegistro.RemoveAt(0);
                    //    }
                    //}
                    eventoItem.ID = ultimoRegistro.Last().ID + 1;

                    if (eventoItem.ID != null)
                        await _client.Child("EventoItem").Child(eventoItem.ID.ToString()).PutAsync(eventoItem);

                    //if (usuario != null)
                    //{
                    //    await _client.Child("EventoItem").Child(usuario.ID.ToString()).PutAsync(new EventoItem { EventoID = eventoItem.ID.Value, Tipo = eventoItem.Tipo, Nome = eventoItem.Nome, QuantidadeDesejada = eventoItem.QuantidadeDesejada, TipoUnidade = eventoItem.TipoUnidade, ID = eventoItem.ID });
                    //}
                }
                else
                    eventoItem.ID = 0;
            }
            return eventoItem;
        }

        // Retorna lista de itens cadastrados pelo criador do evento
        public async Task<List<EventoItem>> GetEventoItemAsync(long eventoID)
        {
            InstaciaClient();
            List<EventoItem> itensEventoRetorno = new List<EventoItem>();
            try
            {
                var eventoItemRetorno = await _client.Child("EventoItem").OrderBy("EventoID").EqualTo(eventoID).OnceSingleAsync<Dictionary<long, EventoItem>>();

                var eventoRetorno =
                    await _client
                        .Child("Eventos")
                        .OrderBy("ID")
                        .EqualTo(eventoID)
                        .OnceSingleAsync<Dictionary<long, Evento>>();

                foreach (EventoItem eventoItem in eventoItemRetorno.Values)
                {
                    itensEventoRetorno.Add(eventoItem);
                }
                
            }
            catch(Exception)
            {
            }
            return itensEventoRetorno;
        }

        #endregion Métodos Child Itens
    }
}
