using Microsoft.EntityFrameworkCore;

namespace mobpsycho.Models
{
    public class MobpsychoDbContext: DbContext
    {
        // Constructor
        public MobpsychoDbContext() { }

        public MobpsychoDbContext(DbContextOptions<MobpsychoDbContext> options) : base(options) { }

        public DbSet<Character> Characters { get; set; }
        public DbSet<Abilitie> Abilities { get; set; }
        public DbSet<User> Users { get; set; }

        // Override names
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>().ToTable("Character");
            modelBuilder.Entity<Abilitie>().ToTable("Abilitie");
            modelBuilder.Entity<User>().ToTable("User");
        }

        // Para corregir error de login usando SHA256, error: 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("mobpsychoLocalDB");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}
