using Billing.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.API
{
    public class DataGenerator
    {
        public static void Initialize(DbContext dbContext)
        {
            dbContext.Add(new User() { Name = "boris", Rating = 5000 });
            dbContext.Add(new User() { Name = "maria", Rating = 1000 });
            dbContext.Add(new User() { Name = "oleg", Rating = 800 });

            dbContext.SaveChanges();
        }
    }
}
