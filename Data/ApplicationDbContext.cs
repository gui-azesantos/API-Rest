using ApiRest.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.Data

{
    public class ApplicationDbContext : DbContext {

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base (options) { }

    }
}