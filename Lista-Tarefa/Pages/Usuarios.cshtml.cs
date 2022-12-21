using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Lista_Tarefa.Pages
{
    public class Usuarios : PageModel
    {
        public List<UsuarioVieModel> UsuariosVM { get; set; }
        private readonly ILogger<Usuarios> _logger;

        public Usuarios(ILogger<Usuarios> logger)
        {
            _logger = logger;
        }

        public async Task OnGet()
        {
            //fazendo conexão com o mysql.                                    //passando o local.
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;port=3306;database=listast_bd;uid=root;password=Pass2022*");
            mySqlConnection.OpenAsync();

            // criando a função pra cria comandos
            MySqlCommand mySqlCommand = mySqlConnection.CreateCommand(); 
            mySqlCommand.CommandText = $"SELECT * FROM  usuarios"; //comando

            MySqlDataReader reader = mySqlCommand.ExecuteReader(); //fazendo leitura na tabela.

            UsuariosVM = new List<UsuarioVieModel>();

            while(await reader.ReadAsync()) //fazendo teste
            {
                UsuariosVM.Add(new UsuarioVieModel
                {
                    Id = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Usuario = reader.GetString(2),
                });
            }

            await mySqlConnection.CloseAsync();
        }
    }

    public class UsuarioVieModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Usuario { get; set; }
    }
}