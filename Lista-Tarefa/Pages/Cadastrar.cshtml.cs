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
    public class Cadastrar : PageModel
    {
        private readonly ILogger<Cadastrar> _logger;

        public Cadastrar(ILogger<Cadastrar> logger)
        {
            _logger = logger;
        }


        [Required(ErrorMessage = "É obrigatorio informar o Nome!")]  
        [BindProperty(SupportsGet = true)]  
        public string Nome { get; set; }


        [Required(ErrorMessage = "É obrigatorio informar o Usuário!")]
        [BindProperty(SupportsGet = true)]
        public string Usuario { get; set; }


        [Required(ErrorMessage = "É obrigatorio informar a Senha!")]
        [DataType(DataType.Password)]
        [BindProperty(SupportsGet = true)]
        public string Senha { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
                        //fazendo conexão com o mysql.             //passando o local.
            MySqlConnection mySqlConnection = new MySqlConnection("server=localhost;port=3306;database=listast_bd;uid=root;password=Pass2022*");
            mySqlConnection.OpenAsync();

            // criando a função pra cria comandos
            MySqlCommand mySqlCommand = mySqlConnection.CreateCommand(); 
            mySqlCommand.CommandText = $"INSERT INTO usuarios (nome, username, senha) VALUES ('{Nome}', '{Usuario}', '{Senha}')"; //comando

           await mySqlCommand.ExecuteReaderAsync(); //fazendo leitura na tabela.

            return new JsonResult(new { Msg = " Usuario Cadastrado com sucesso!" });
        }
    }
}