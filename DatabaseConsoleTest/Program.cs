using FireSharp;
using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseConsoleTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            Usuario usr = new Usuario
            {
                ID = null,
                Nome = "Nome",
                Celular = "11922222222",
                Email = "email",
                Senha = "senha"
            };

            var client = new FirebaseConfig<Usuario>();

            //var a = await client.GetListAsync("Usuarios");

            await client.InsertAsync("Usuarios", usr);

            Console.ReadKey();
        }
    }
}
