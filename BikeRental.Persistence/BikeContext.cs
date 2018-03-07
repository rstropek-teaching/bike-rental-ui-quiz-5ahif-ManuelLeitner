using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BikeRental.Persistence {
    public class BikeContext : DbContext {

        public readonly string ConnectionString;

        public BikeContext(string connectionString = "") {
            ConnectionString = connectionString;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                if (string.IsNullOrWhiteSpace(ConnectionString)) {
                    optionsBuilder.UseInMemoryDatabase("InMemoryDB");
                } else {
                    optionsBuilder.UseSqlServer(ConnectionString);
                }
            }
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Category> Category { get; set; }

    }

    public class ContextFactory : IDesignTimeDbContextFactory<BikeContext> {

        public BikeContext CreateDbContext(string[] args) {
            var builder = new ConfigurationBuilder().SetBasePath(Path.GetFullPath("../BikeRental"));
            builder.AddJsonFile("appsettings.json").AddEnvironmentVariables();

            var config = builder.Build();
            return new BikeContext(config.GetConnectionString("Azure"));
        }
    }
}
