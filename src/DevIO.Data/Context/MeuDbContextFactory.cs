using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace DevIO.Data.Context
{
    public class MeuDbContextFactory : IDesignTimeDbContextFactory<MeuDbContext>
    {
        public MeuDbContext CreateDbContext(string[] args)
        {                  
            var optionsBuilder = new DbContextOptionsBuilder();

            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new MeuDbContext(optionsBuilder.Options);
        }
    }
}
