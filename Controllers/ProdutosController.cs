using System;
using System.Collections.Generic;
using System.Linq;
using ApiRest.Data;
using ApiRest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiRest.Controllers {
    [Route ("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ProdutosController : ControllerBase {

        private readonly ApplicationDbContext database;

        public ProdutosController (ApplicationDbContext database) {

            this.database = database;

        }
        [HttpGet("teste")]
        public IActionResult TesteClaims(){
         return Ok (HttpContext.User.Claims.First(claim => claim.Type.ToString().Equals("id", StringComparison.InvariantCultureIgnoreCase)).Value);
        }

        [HttpGet]
        public IActionResult Get () {
            var produtos = database.Produtos.ToList ();

            return Ok (produtos);

        }

        [HttpGet ("{id}")]
        public IActionResult Get (int id) {
            try {
                Produto produto = database.Produtos.First (p => p.Id == id);

                return Ok (produto);
            } catch (Exception) {

                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "Id inválido" });
            }

        }

        [HttpPost]
        public IActionResult Post ([FromBody] ProdutoTemp pTemp) {
            //Validação
            if (pTemp.Preco <= 0) {
                Response.StatusCode = 400;
                return new ObjectResult (new { msg = "O preço do produto não pode ser menor ou igual a zero!" });
            }
            if (pTemp.Nome.Length <= 1) {
                Response.StatusCode = 400;
                return new ObjectResult (new { msg = "O nome do produto não pode ser vazio!" });

            }

            Produto p = new Produto ();
            p.Nome = pTemp.Nome;
            p.Preco = pTemp.Preco;
            database.Produtos.Add (p);
            database.SaveChanges ();

            //Set do Status Code
            Response.StatusCode = 201;
            return new ObjectResult (new { msg = "Produto criado com sucesso!" });
        }

        [HttpDelete ("{id}")]
        public IActionResult Delete (int id) {
            try {
                var produto = database.Produtos.First (p => p.Id == id);
                database.Produtos.Remove (produto);
                database.SaveChanges ();
                return Ok (produto);
            } catch (Exception) {

                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "Id inválido" });
            }
        }

        [HttpPatch]
        public IActionResult Patch ([FromBody] Produto produto) {
            if (produto.Id > 0) {

                try {
                    var ptemp = database.Produtos.First (p => p.Id == produto.Id);
                    if (ptemp != null) {
                        //Editar 
                        //Condição ? Faz Algo : Faz outra coisa
                        ptemp.Nome = produto.Nome != null ? produto.Nome : ptemp.Nome;
                        ptemp.Preco = produto.Preco != 0 ? produto.Preco : ptemp.Preco;

                        database.SaveChanges ();
                        return Ok ();
                    }
                } catch (System.Exception) {
                    Response.StatusCode = 404;
                    return new ObjectResult (new { msg = "Produto não encontrado" });
                }

            } else {
                Response.StatusCode = 404;
                return new ObjectResult (new { msg = "Id inválido" });
            }
            Response.StatusCode = 404;
            return new ObjectResult (new { msg = "Id inválido" });
        }
        public class ProdutoTemp {
            public string Nome { get; set; }
            public float Preco { get; set; }
        }

    }
}