using FinBeat.DAL.Configurations;
using FinBeat.DAL.Extensions;
using FinBeat.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace FinBeat.DAL
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// A DbSet representing the collection of Entity objects stored in the database.
        /// </summary>
        public DbSet<Entity> Entities { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// Configures the database context options, enabling sensitive data logging and detailed errors.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
        }

        /// <summary>
        /// Configures the entity model, applying entity configurations and converting names to snake case.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for the database.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration of FinBeatTech models.
            modelBuilder.ApplyConfiguration(new EntityConfiguration());

            modelBuilder.ToSnakeCaseMigration();
        }
    }
}
