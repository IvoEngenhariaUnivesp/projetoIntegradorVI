using System;
using System.Threading.Tasks;
using FireSharp.Interfaces;
using FireSharp;
using ProjetoIntegradorVI.Model.Common;
using System.Collections;
using System.Collections.Generic;
using FireSharp.Response;
using System.Net;
using System.Linq;

namespace ProjetoIntegradorVI.Database
{
    public class FirebaseConfig<T> where T : EntidadeBase
    {
        private IFirebaseClient _client;

        /// <summary>
        /// Configura a Instância do banco
        /// </summary>
        private static IFirebaseConfig GetConfiguration()
        {
            return new FireSharp.Config.FirebaseConfig
            {
                AuthSecret = "em8oKbMoEJvVxipNAMW9ZPItgWHxO3P9iW04VLGf",
                BasePath = "https://projetointegradorvi-4n1.firebaseio.com/"
            };
        }


        /// <summary>
        /// Abre a conexão instanciando a configuração no client do Firebase
        /// </summary>
        private void OpenConnection()
        {
            var config = GetConfiguration();
            try
            {
                _client = new FirebaseClient(config);

                if (_client == null)
                    throw new Exception("Conexão Rejeitada");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Método de inserção genérico
        /// </summary>
        /// <param name="table">Nome da tabela no banco de dados</param>
        /// <param name="objeto">Objeto a ser inserido</param>
        /// <param name="objetoID">ID do objeto a ser inserido</param>
        /// <returns name="T">Retorno do tipo informado na instância do FirebaseConfig</returns>
        public async Task<T> InsertAsync(string table, T objeto)
        {
            // Abre a conexão com o banco
            OpenConnection();

            // Pega o último registro da tabela informada
            var ultimoRegistro = await _client.GetAsync("Usuarios", QueryBuilder.New().OrderBy("ID").LimitToLast(1));

            // Instancia o ID
            long ID = 0;

            // Se não existir nenhum objeto, o ID é 1, se não, pega o ID e adiciona +1
            if (ultimoRegistro.Body == "null")
                ID = 1;
            else if (ultimoRegistro.Body.Contains("[null"))
                ID = ultimoRegistro.ResultAs<T[]>()[1].ID.Value + 1;

            // Verifica se já existe um objeto com o mesmo ID no banco
            // O Firebase não faz essa verificação, só edita o objeto direto
            var objectResponse = await _client.GetAsync(table + "/" + ID);

            // Se não existe um objeto com o ID, insere no banco
            if (objectResponse.Body == "null")
            {
                objeto.ID = ID;
                _client.Set(table + "/" + ID, objeto);
            }
            else
            {
                // Para fins de concorrência, se o objeto existir, incrementa o ID e faz uma nova busca
                while (true)
                {
                    ID++;
                    objectResponse = await _client.GetAsync(table + "/" + ID);
                    if (objectResponse.Body == "null")
                    {
                        objeto.ID = ID;
                        _client.Set(table + "/" + ID, objeto);
                        break;
                    }
                }
            }

            return objeto;
        }

        /// <summary>
        /// Método de edição genérico
        /// </summary>
        /// <param name="table">Nome da tabela no banco de dados</param>
        /// <param name="objeto">Objeto a ser editado</param>
        /// <param name="objetoID">ID do objeto a ser editado</param>
        /// <returns name="T">Retorno do tipo informado na instância do FirebaseConfig</returns>
        public async Task<T> UpdateAsync(string table, T objeto, long objetoID)
        {
            // Abre a conexão com o banco
            OpenConnection();

            try
            {
                // Verifica se já existe um objeto com o mesmo ID no banco
                // O Firebase não faz essa verificação, só edita o objeto direto
                var objectResponse = await _client.GetAsync(table + "/" + objetoID);

                // Se o objeto não existir, lança exception e retorna null
                if (objectResponse.Body == "null")
                    throw new Exception();

                // Edita o objeto no banco
                _client.Set(table + "/" + objetoID, objeto);
            }
            catch (Exception)
            {
                // Se tiver errro, retorna null
                return default(T);
            }

            // Se não houver problemas, retorna o objeto editado
            return objeto;
        }

        /// <summary>
        /// Método de exclusão genérico
        /// </summary>
        /// <param name="table">Nome da tabela no banco de dados</param>
        /// <param name="objetoID">ID do objeto a ser excluido</param>
        /// <returns name="bool">Retorno do tipo informado na instância do FirebaseConfig</returns>
        public async Task<bool> DeleteAsync(string table, long objetoID)
        {
            // Abre a conexão com o banco
            OpenConnection();

            try
            {
                // Verifica se já existe um objeto com o mesmo ID no banco
                // O Firebase não faz essa verificação, só edita o objeto direto
                var objectResponse = await _client.GetAsync(table + "/" + objetoID);

                // Se o objeto não existir, lança exception e retorna null
                if (objectResponse.Body == "null")
                    throw new Exception();

                // Edita o objeto no banco
                await _client.DeleteAsync(table + "/" + objetoID);
            }
            catch (Exception)
            {
                // Se tiver errro, retorna null
                return false;
            }

            // Se não houver problemas, retorna o objeto editado
            return true;
        }

        /// <summary>
        /// Método de retorno genérico
        /// </summary>
        /// <param name="table">Tabela do objeto a ser buscado</param>
        /// <param name="objetoID"> ID do objeto a ser buscado</param>
        /// <returns></returns>
        public async Task<T> GetAsync(string table, long objetoID)
        {
            // Abre a conexão com o banco
            OpenConnection();

            try
            {
                // Verifica se já existe um objeto com o mesmo ID no banco
                // O Firebase não faz essa verificação, só edita o objeto direto
                var objectResponse = await _client.GetAsync(table + "/" + objetoID);

                // Se o objeto não existir, lança exception e retorna null
                if (objectResponse.Body == "null")
                    throw new Exception();

                return objectResponse.ResultAs<T>();
            }
            catch (Exception)
            {
                // Se tiver errro, retorna null
                return default(T);
            }
        }

        public async Task<List<T>> GetListAsync(string table, QueryBuilder queryBuilder = null)
        {
            // Abre a conexão com o banco
            OpenConnection();

            try
            {
                FirebaseResponse objectResponse = new FirebaseResponse("null", HttpStatusCode.OK);
                // Verifica se já existe um objeto com o mesmo ID no banco
                // O Firebase não faz essa verificação, só edita o objeto direto
                if (queryBuilder != null)
                    objectResponse = await _client.GetAsync(table, queryBuilder);
                else
                    objectResponse = await _client.GetAsync(table);

                // Se o objeto não existir, lança exception e retorna null
                if (objectResponse.Body == "null")
                    throw new Exception();

                var retorno = objectResponse.ResultAs<List<T>>().Where(x => x != null).ToList();

                return retorno;
            }
            catch (Exception)
            {
                // Se tiver errro, retorna null
                return default(List<T>);
            }
        }

    }
}
