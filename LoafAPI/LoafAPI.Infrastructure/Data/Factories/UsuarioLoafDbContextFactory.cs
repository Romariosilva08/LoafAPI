using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace LoafAPI.LoafAPI.Infrastructure.Data
{
    public class UsuarioLoafDbContextFactory : IDesignTimeDbContextFactory<UsuarioLoafDbContext>
    {
        public UsuarioLoafDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<UsuarioLoafDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlite(connectionString);

            return new UsuarioLoafDbContext(builder.Options);
        }
    }
}
