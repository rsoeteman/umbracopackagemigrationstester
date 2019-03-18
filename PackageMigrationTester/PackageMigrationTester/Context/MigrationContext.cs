using System;
using System.Collections.Generic;
using Umbraco.Core.Logging;
using Umbraco.Core.Migrations;
using Umbraco.Core.Persistence;

namespace PackageMigrationTester.Context
{
    public class PackageMigrationContext : IMigrationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationContext"/> class.
        /// </summary>
        public PackageMigrationContext(IUmbracoDatabase database, ILogger logger)
        {
            Database = database ?? throw new ArgumentNullException(nameof(database));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public ILogger Logger { get; }

        /// <inheritdoc />
        public IUmbracoDatabase Database { get; }

        /// <inheritdoc />
        public ISqlContext SqlContext => Database.SqlContext;

        /// <inheritdoc />
        public int Index { get; set; }

        /// <inheritdoc />
        public bool BuildingExpression { get; set; }

        // this is only internally exposed
        public List<Type> PostMigrations { get; } = new List<Type>();

        /// <inheritdoc />
        public void AddPostMigration<TMigration>()
            where TMigration : IMigration
        {
            PostMigrations.Add(typeof(TMigration));
        }
    }
}