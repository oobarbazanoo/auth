using Infrastructure.Data.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//migrations are generated from the Infrastructure project using the command:
//dotnet ef --startup-project ../API/ migrations add InitialCreate -o Data/Migrations

namespace Infrastructure.Data.Configuration
{
    public class AuthDbContext : DbContext
    {
        readonly IConfiguration Configuration;

        public AuthDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public DbSet<User> Users { get; set; }
    }
}