using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ProjetoIntegradorVI.Domain.Model.Common;
using ProjetoIntegradorVI.Domain.Model;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq.Expressions;

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
                    .OnceSingleAsync<KeyValuePair<long, Usuario>>();

            // Caso retorne null, quer dizer que não existe usuário com o mesmo email
            if (usuarioRetorno.Value == null)
            {
                // Atribui ao objeto o ultimo ID do banco + 1
                try
                {
                    // Liberado para cadastrar
                    // Pega ultimo registro do banco em forma de dicionario. Ex: Key: ID - Values = Usuario
                    var ultimoRegistro = await _client
                        .Child("Usuarios")
                        .OrderBy("ID")
                        .LimitToLast(1)
                        .OnceSingleAsync<KeyValuePair<long, Usuario>>();

                    objeto.ID = ultimoRegistro.Key + 1;
                }
                catch (Exception)
                {
                    objeto.ID = 1;
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

    }
}
