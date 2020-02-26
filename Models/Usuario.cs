using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace ApiRest.Models {
        public class Usuario{
        public int Id { get; set; }

        public string Senha { get; set; }

        public string Email { get; set; }
    }
}