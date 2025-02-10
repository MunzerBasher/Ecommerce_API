
using EcommerceDataLayer.Entities.Roles;
using EcommerceDataLayer.Entities.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;


namespace EcommerceDataLayer.AppDbContex
{
    public class ApplicationDbContext : IdentityDbContext<UserIdentity, UserRoles,string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-LMLA8GF\\MSSQLSERVERNEW;Database=Ecommerce;User Id=sa;Password=sa123456;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }

    public class AppDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // Build the configuration (ensure this matches your appsettings.json or other config sources)
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Adjust path if needed
                .AddJsonFile("appsettings.json")  // Make sure this file is available at design time
                .Build();

            // Create the DbContextOptions with the connection string from the configuration
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = config.GetConnectionString("DefaultConnection");  // Replace with your connection string name
            optionsBuilder.UseSqlServer(connectionString); // or UseSqlite, UseNpgsql depending on your DB

            return new ApplicationDbContext(optionsBuilder.Options);
        }











    }
}
