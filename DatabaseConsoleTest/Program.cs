using ProjetoIntegradorVI.Database;
using ProjetoIntegradorVI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
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
                ID = 1,
                Nome = "Ivo",
                Celular = "958576549",
                Email = "rennancfra@gmail.com",
                Senha = "Rennan-jinkk12"
            };

            var client = new FirebaseConfig<Usuario>();

            var retornoUsuario = await client.Insert("Usuarios", usr, usr.ID);

            Console.ReadKey();
        }
    }
}
