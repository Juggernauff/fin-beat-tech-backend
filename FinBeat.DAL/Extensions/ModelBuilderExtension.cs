using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text.RegularExpressions;

namespace FinBeat.DAL.Extensions
{
    public static class ModelBuilderExtension
    {
        /// <summary>
        /// Converts a string to snake_case style.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>A string in snake_case format.</returns>
        private static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            return Regex.Match(input, "^_+")?.ToString() + Regex.Replace(input, "([a-z])([0-9A-Z])", "$1_$2").ToLower();
        }

        /// <summary>
        /// Updates table names in the data model, converting them to snake_case style.
        /// </summary>
        /// <param name="modelBuilder">The data model to update.</param>
        private static void UpdateTableNames(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.GetTableName().ToSnakeCase());
            }
        }

        /// <summary>
        /// Updates column names in the data model, converting them to snake_case style.
        /// </summary>
        /// <param name="modelBuilder">The data model to update.</param>
        private static void UpdateColumnNames(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableProperty property in entityType.GetProperties())
                {
                    StoreObjectIdentifier? storeObjectIdentifier = StoreObjectIdentifier.Create(property.DeclaringEntityType, StoreObjectType.Table);
                    StoreObjectIdentifier storeObject = storeObjectIdentifier.GetValueOrDefault();
                    if (property.GetColumnName(in storeObject) == property.Name)
                    {
                        property.SetColumnName(property.Name.ToSnakeCase());
                    }
                }
            }
        }

        /// <summary>
        /// Updates key names in the data model, converting them to snake_case style.
        /// </summary>
        /// <param name="modelBuilder">The data model to update.</param>
        private static void UpdateKeyNames(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableKey key in entityType.GetKeys())
                {
                    key.SetName(key.GetName().ToSnakeCase());
                }
            }
        }

        /// <summary>
        /// Updates foreign key names in the data model, converting them to snake_case style.
        /// </summary>
        /// <param name="modelBuilder">The data model to update.</param>
        private static void UpdateForeignKeyNames(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableForeignKey foreignKey in entityType.GetForeignKeys())
                {
                    foreignKey.SetConstraintName(foreignKey.GetConstraintName().ToSnakeCase());
                }
            }
        }

        /// <summary>
        /// Updates index names in the data model, converting them to snake_case style.
        /// </summary>
        /// <param name="modelBuilder">The data model to update.</param>
        private static void UpdateIndexNames(ModelBuilder modelBuilder)
        {
            foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (IMutableIndex index in entityType.GetIndexes())
                {
                    index.SetDatabaseName(index.GetDatabaseName().ToSnakeCase());
                }
            }
        }

        /// <summary>
        /// Translates all table, column, key, foreign key, and index names in the model to snake case for PostgreSQL or other database conventions.
        /// </summary>
        public static void ToSnakeCaseMigration(this ModelBuilder modelBuilder)
        {
            UpdateTableNames(modelBuilder);
            UpdateColumnNames(modelBuilder);
            UpdateKeyNames(modelBuilder);
            UpdateForeignKeyNames(modelBuilder);
            UpdateIndexNames(modelBuilder);
        }
    }
}
