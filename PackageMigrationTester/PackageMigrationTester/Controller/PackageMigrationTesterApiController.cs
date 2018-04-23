using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using PackageMigrationTester.Installer.Migrations;
using PackageMigrationTester.Model;
using Umbraco.Core;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;
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
                    from migrationAttribute in migration.GetCustomAttributes<MigrationAttribute>() 
                    select migrationAttribute.ProductName)
                    .Where(s=> !s.Equals("Umbraco",StringComparison.InvariantCultureIgnoreCase))
                    .Distinct()
                    .OrderBy(s=>s);
        }

        [HttpGet]
        public IEnumerable<MigrationInfo> GetMigrations(string packageName)
        {
            var result = new List<MigrationInfo>();
            var migrations = TypeFinder.FindClassesOfType<MigrationBase>();
            foreach (var migration in migrations)
            {
                foreach (var migrationAttribute in migration.GetCustomAttributes<MigrationAttribute>().Where(att=> att.ProductName.Equals(packageName)))
                {
                    result.Add(  new MigrationInfo {MigrationClassName = migration.FullName, TargetVersion = migrationAttribute.TargetVersion.ToString()});
                }
            }

            return result.OrderBy(r=>r.TargetVersion);
        }

        public void Up(MigrationInfo migrationToExecute)
        {
            var  migrationObject =Activator.CreateInstance(Type.GetType(migrationToExecute.MigrationClassName),new object[] {ApplicationContext.DatabaseContext.SqlSyntax,Logger}) as MigrationBase;
            migrationObject.Up();
        }

        public void Down(MigrationInfo migrationToExecute)
        {
            var  migrationObject =Activator.CreateInstance(Type.GetType(migrationToExecute.MigrationClassName),new object[] {ApplicationContext.DatabaseContext.SqlSyntax,Logger}) as MigrationBase;
            migrationObject.Down();
        }
    }
}
