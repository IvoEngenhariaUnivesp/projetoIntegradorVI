using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp;
using System.Threading;

namespace ProjetoIntegradorVI.Database
{
    public class FirebaseConfig<T>
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
        public async Task<T> InsertAsync(string table, T objeto, long objetoID)
        {
            // Abre a conexão com o banco
            OpenConnection();

            try
            {
                // Verifica se já existe um objeto com o mesmo ID no banco
                // O Firebase não faz essa verificação, só edita o objeto direto
                var objectResponse = await _client.GetAsync(table + "/" + objetoID);

                // Se o objeto existir, lança exception e retorna null
                if (objectResponse.Body != "null")
                    throw new Exception();

                _client.Set(table + "/" + objetoID, objeto);
            }
            catch (Exception)
            {
                return default(T);
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
                var objectResponse = await _client.GetAsync(table + "/" + 1);

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

    }
}
