using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace Lista_Tarefa.Controllers
{
    //[Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return Json(new { Msg = "Usuário Já Logado !"} );
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logar(string username, string senha, bool manterlogado)
        {
            //fazendo conexão com o mysql.                                    //passando o local.
            MySqlConnection mySqlConnection = new MySqlConnection("");
            mySqlConnection.OpenAsync();

            // criando a função pra cria comandos
            MySqlCommand mySqlCommand = mySqlConnection.CreateCommand(); 
            mySqlCommand.CommandText = $"SELECT * FROM  usuarios WHERE username = '{username}' AND senha = '{senha}'"; //comando

            MySqlDataReader reader = mySqlCommand.ExecuteReader(); //fazendo leitura na tabela.

          
                if(await reader.ReadAsync()) //fazendo teste
                {
                    int usuarioid = reader.GetInt32(0);
                    string nome = reader.GetString(1); 

                     List<Claim> direitosAcesso =new List<Claim>
                     {
                        new Claim(ClaimTypes.NameIdentifier,usuarioid.ToString()),
                        new Claim(ClaimTypes.Name,nome)
                     }; 

                    var identity = new ClaimsIdentity(direitosAcesso,"Identity.Login");
                    var userPrincial = new ClaimsPrincipal(new[] { identity });

                    await HttpContext.SignInAsync(userPrincial,
                    new AuthenticationProperties
                    {
                        IsPersistent = manterlogado,
                        ExpiresUtc = DateTime.Now.AddHours(1)
                    });

                    return Json(new { Msg = " Usuario Logado com sucesso !" });
                }
            
            
            return Json(new { Msg = " Usuario não encontrado ! Verifique as credenciais!" });
            
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }
            return RedirectToAction("Index","Login");
        }
    }
}