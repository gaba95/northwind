using System.Data.Entity;
using Northwind.Models;

namespace Northwind.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("DefaultConnection") { }

        public DbSet<Customer> Customers { get; set; }
    }
}
