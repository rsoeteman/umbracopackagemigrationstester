using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PackageMigrationTester.Context;
using PackageMigrationTester.Model;
using Umbraco.Core;
using Umbraco.Core.Composing;
using Umbraco.Core.Migrations;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace PackageMigrationTester.Controller
{
    [PluginController("packagemigrationtester")]
    public class PackageMigrationTesterApiController : UmbracoAuthorizedApiController
    {

        [HttpGet]
        public IEnumerable<string> Initialize()
        {
            return 
                (from migration in TypeFinder.FindClassesOfType<MigrationBase>()
                    select migration.Assembly.GetName().Name)
                    .Where(s=> !s.StartsWith("Umbraco.core",StringComparison.InvariantCultureIgnoreCase))
                    .Distinct()
                    .OrderBy(s=>s);
        }

        [HttpGet]
        public IEnumerable<MigrationInfo> GetMigrations(string packageName)
        {
            var result = new List<MigrationInfo>();
            var migrations = TypeFinder.FindClassesOfType<MigrationBase>();
            foreach (var migration in migrations.Where( m=>m.Assembly.GetName().Name == packageName))
            {
               
               result.Add(  new MigrationInfo {MigrationClassName = migration.FullName,AssemblyQualifiedName = migration.AssemblyQualifiedName});
               
            }

            return result;
        }

        public void Migrate(MigrationInfo migrationToExecute)
        {
            try
            {
                using (var scope = Current.ScopeProvider.CreateScope())
                {
                    var context = new PackageMigrationContext(scope.Database,Logger);

                    var migrationType = Type.GetType(migrationToExecute.AssemblyQualifiedName);
                    var migrationObject = Activator.CreateInstance(migrationType,new object[] {context, Current.Logger}) as MigrationBase;
                    migrationObject.Migrate();
                    scope.Complete();
                }

              
            }
            catch (Exception ex)
            {
                Current.Logger.Error(typeof(PackageMigrationTesterApiController),ex);
                throw ;
            }
        }
        }
}
