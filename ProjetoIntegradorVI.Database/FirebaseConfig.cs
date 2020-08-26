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
    public class FirebaseConfig
    {
        private static IFirebaseConfig GetConfiguration()
        {
            return new FireSharp.Config.FirebaseConfig
            {
                AuthSecret = "em8oKbMoEJvVxipNAMW9ZPItgWHxO3P9iW04VLGf",
                BasePath = "https://projetointegradorvi-4n1.firebaseio.com/"
            };
        }

        public static IFirebaseClient GetInstanceConnection()
        {
            IFirebaseConfig config = GetConfiguration();
            IFirebaseClient client;
            try
            {
                client = new FirebaseClient(config);

                if (client == null)
                    throw new Exception("Conexão Rejeitada");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return client;
        }
    }
}
