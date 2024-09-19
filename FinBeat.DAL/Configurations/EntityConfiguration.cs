using FinBeat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinBeat.DAL.Configurations
{
    public class EntityConfiguration : IEntityTypeConfiguration<Entity>
    {
        /// <summary>
        /// Configures the entity properties and relationships.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity.</param>
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            builder.ToTable(nameof(Entity));

            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id)
                .ValueGeneratedOnAdd();

            builder.Property(dmc => dmc.Value).HasMaxLength(1024);
        }
    }
}
