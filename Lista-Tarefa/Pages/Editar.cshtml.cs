using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Lista_Tarefa.Pages
{
    public class Editar : PageModel
    {
        private readonly ILogger<Editar> _logger;

        public Editar(ILogger<Editar> logger)
        {
            _logger = logger;
        }

        [BindProperty(SupportsGet = true)] 
        public int Id { get; set; }

        [Required(ErrorMessage = "É obrigatorio informar o Nome!")]  
        [BindProperty(SupportsGet = true)]  
        public string Nome { get; set; }


        [Required(ErrorMessage = "É obrigatorio informar o Usuário!")]
        [BindProperty(SupportsGet = true)]
        public string Usuario { get; set; }
        public async Task OnGet()
        {
             //fazendo conexão com o mysql.                                    //passando o local.
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;port=3306;database=listast_bd;uid=root;password=Pass2022*");
            mySqlConnection.OpenAsync();

            // criando a função pra cria comandos
            MySqlCommand mySqlCommand = mySqlConnection.CreateCommand(); 
            mySqlCommand.CommandText = $"SELECT * FROM  usuarios WHERE id = '{Id}'"; //comando

            MySqlDataReader reader = mySqlCommand.ExecuteReader(); //fazendo leitura na tabela.

          
                if(await reader.ReadAsync()) //fazendo teste
                {   
                    Nome = reader.GetString(1);
                    Usuario = reader.GetString(2); 
                }

                await mySqlConnection.CloseAsync();
        }
        public async Task<IActionResult> OnPost()
        {
               //fazendo conexão com o mysql.                                    //passando o local.
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;port=3306;database=listast_bd;uid=root;password=Pass2022*");
            mySqlConnection.OpenAsync();

            // criando a função pra cria comandos
            MySqlCommand mySqlCommand = mySqlConnection.CreateCommand(); 
            mySqlCommand.CommandText = $"UPDATE usuarios SET username = '{Usuario}', Nome = '{Nome}' WHERE id = '{Id}' "; //comando

            await mySqlCommand.ExecuteReaderAsync(); //fazendo leitura na tabela.

            return new JsonResult(new { Msg = " Usuario Editado com sucesso!" });
        }

        public async Task<IActionResult> OnGetApagar()
        {
                           //fazendo conexão com o mysql.                                    //passando o local.
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;port=3306;database=listast_bd;uid=root;password=Pass2022*");
            mySqlConnection.OpenAsync();

            // criando a função pra cria comandos
            MySqlCommand mySqlCommand = mySqlConnection.CreateCommand(); 
            mySqlCommand.CommandText = $"DELETE FROM usuarios WHERE id = '{Id}' "; //comando

            await mySqlCommand.ExecuteReaderAsync(); //fazendo leitura na tabela.
            await mySqlConnection.CloseAsync();


            return new JsonResult(new { Msg = " Usuario Removido com sucesso!" });
        }
    }
}