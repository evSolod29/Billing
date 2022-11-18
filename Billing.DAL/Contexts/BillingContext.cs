using Billing.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.DAL.Contexts
{
    public class BillingContext : DbContext
    {
        public BillingContext(DbContextOptions options) : base(options) 
        { 

        }
        
        public DbSet<User> Users => Set<User>();
        public DbSet<Coin> Coins => Set<Coin>();
        public DbSet<History> Histories => Set<History>();
    }
}
