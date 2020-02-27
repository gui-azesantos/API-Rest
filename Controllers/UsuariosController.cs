using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ApiRest.Data;
using ApiRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace ApiRest.Controllers {
    [Route ("api/v1/[controller]")]
    [ApiController]

    public class UsuariosController : ControllerBase {
        private readonly ApplicationDbContext database;

        public UsuariosController (ApplicationDbContext database) {

            this.database = database;

        }

        [HttpPost ("registro")]
        public IActionResult Registro ([FromBody] Usuario usuarios) {
            database.Add (usuarios);
            database.SaveChanges ();
            return Ok (new { msg = "Usuário cadastrado com sucesso" });
        }

        [HttpPost ("login")]
        public IActionResult Login ([FromBody] Usuario credenciais) {

            Usuario user = database.Usuarios.First (user => user.Email.Equals (credenciais.Email));
            if (user != null) {
                if (user.Senha.Equals (credenciais.Senha)) {
                    string Token = "Testeabcdefghijklmno_pqrstuvwxyzz"; //Chave de Segurança

                    var claims = new List<Claim> ();
                    var ChaveSimetrica = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (Token));
                    var CredenciaisDeAcesso = new SigningCredentials (ChaveSimetrica, SecurityAlgorithms.HmacSha256Signature);
                    claims.Add (new Claim ("Id", user.Id.ToString ()));
                    claims.Add (new Claim ("Email", user.Email.ToString ()));
                    claims.Add (new Claim (ClaimTypes.Role, "Admin"));

                    var JWT = new JwtSecurityToken (
                        issuer: "Teste.com", //Quem está fornecendo o Jwt
                        expires : DateTime.Now.AddHours (1),
                        audience: "User",
                        signingCredentials : CredenciaisDeAcesso,
                        claims : claims
                    );

                    Response.StatusCode = 200;
                    return new ObjectResult (new JwtSecurityTokenHandler ().WriteToken (JWT));
                } else {
                    Response.StatusCode = 401; //Não Autorizado
                    return new ObjectResult ("1");
                }
            } else {
                Response.StatusCode = 401; //Não Autorizado
                return new ObjectResult ("2");
            }

        }

    }
}