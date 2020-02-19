using System;
using System.Linq;
using ApiRest.Data;
using ApiRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiRest.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase {

        private readonly ApplicationDbContext database;

        public ProdutosController (ApplicationDbContext database) {
            this.database = database;
        }

        [HttpGet]
        public IActionResult Get () {
            var produtos = database.Produtos.ToList ();
            return Ok (produtos); //Status code = 200 && Dados 
        }

        [HttpGet ("{id}")]
        public IActionResult Get (int id) {
            try {
                var produtos = database.Produtos.First (p => p.Id == id);
                return Ok (produtos);
            } catch (Exception) {
                return BadRequest (new { msg = "Id inválido" });
            }

        }

        [HttpPost]
        public IActionResult Post ([FromBody] ProdutoTemp pTemp) {
            Produto p = new Produto ();
            p.Nome = pTemp.Nome;
            p.Preco = pTemp.Preco;
            database.Produtos.Add (p);
            database.SaveChanges ();
            Response.StatusCode = 201;
            return new ObjectResult (new { msg = "Produto criado com sucesso!" });
        }

        public class ProdutoTemp {
            public string Nome { get; set; }
            public float Preco { get; set; }
        }
    }
}