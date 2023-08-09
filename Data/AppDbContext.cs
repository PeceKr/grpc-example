using Microsoft.EntityFrameworkCore;
using GRPCExample.Models;
namespace GRPCExample.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products => Set<Product>();
    }
}