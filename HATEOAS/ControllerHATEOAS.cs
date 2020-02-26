/* using System;
using System.Collections.Generic;
using System.Linq;
using ApiRest.Data;
using ApiRest.HATEOAS;
using ApiRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiRest.Controllers {
    [Route ("api/v1/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase {

        private readonly ApplicationDbContext database;
        private HATEOAS.HATEOAS HATEOAS;

        public ProdutosController (ApplicationDbContext database) {

            this.database = database;
            HATEOAS = new HATEOAS.HATEOAS ("localhost:5001/api/v1/produtos");
            HATEOAS.AddAction ("GET_INFO", "GET");
            HATEOAS.AddAction ("DELETE_PRODUCT", "DELETE");
            HATEOAS.AddAction ("EDIT_PRODUCT", "PATCH");
        }

        [HttpGet]
        public IActionResult Get () {
            var produtos = database.Produtos.ToList ();
            List<ProdutoContainer> produtosHateoas = new List<ProdutoContainer> ();
            foreach (var prod in produtos) {
                ProdutoContainer produtoHateoas = new ProdutoContainer ();
                produtoHateoas.produto = prod;
                produtoHateoas.links = HATEOAS.GetActions (prod.Id.ToString ());
                produtosHateoas.Add (produtoHateoas);
                return Ok (produtoHateoas);
                
            }
            return Ok (produtos); //Status code = 200 && Dados 
        }

        [HttpGet ("{id}")]
        public IActionResult Get (int id) {
            try {
                Produto produto = database.Produtos.First (p => p.Id == id);
                ProdutoContainer produtoHATEOAS = new ProdutoContainer ();
                //HATEOAS.AddAction(GET_INFO)
                produtoHATEOAS.produto = produto;
                produtoHATEOAS.links = HATEOAS.GetActions (produto.Id.ToString ());
                return Ok (produtoHATEOAS);
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
        public class ProdutoContainer {
            public Produto produto;
            public Link[] links;

        }
    }
} */