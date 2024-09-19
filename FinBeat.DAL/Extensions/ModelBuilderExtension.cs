using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.RegularExpressions;

namespace FinBeat.DAL.Extensions
{
    public static class ModelBuilderExtension
    {
        private static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return Regex.Match(input, "^_+")?.ToString() + Regex.Replace(input, "([a-z])([0-9A-Z])", "$1_$2").ToLower();
        }

        /// <summary>
        /// Translates all table, column, key, foreign key, and index names in the model to snake case for PostgreSQL or other database conventions.
        /// </summary>
        public static void ToSnakeCaseMigration(this ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.GetTableName().ToSnakeCase());
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    StoreObjectIdentifier? storeObjectIdentifier = StoreObjectIdentifier.Create(property.DeclaringEntityType, StoreObjectType.Table);
                    StoreObjectIdentifier storeObject = storeObjectIdentifier.GetValueOrDefault();
                    if (property.GetColumnName(in storeObject) == property.Name)
                    {
                        property.SetColumnName(property.Name.ToSnakeCase());
                    }
                }

                foreach (IMutableKey key in entityType.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }

                foreach (IMutableForeignKey foreignKey in entityType.GetForeignKeys())
                {
                    foreignKey.SetConstraintName(foreignKey.GetConstraintName().ToSnakeCase());
                }

                foreach (IMutableIndex index in entityType.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
                }
            }
        }
    }
}
