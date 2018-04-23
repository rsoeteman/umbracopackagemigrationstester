using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PackageMigrationTester.Installer.Actions;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence.Migrations;
using Umbraco.Core.Persistence.SqlSyntax;

namespace PackageMigrationTester.Installer.Migrations
{
    [Migration("1.0.0", 1, "PackageMigrationTester")]
    public class InstallPackageTesterDashboardMigration : MigrationBase
    {
        public InstallPackageTesterDashboardMigration(ISqlSyntaxProvider sqlSyntax, ILogger logger) : base(sqlSyntax, logger)
        {
        }

        public override void Up()
        {
            var action = GetActionConfig();
            action.Install();
        }

        public override void Down()
        {
            var action = GetActionConfig();
            action.UnInstall();
        }

        private InstallDashboardAction GetActionConfig()
        {
            return new InstallDashboardAction { Area = "developer", SectionAlias = "PackageMigrationsTester", TabCaption = "Package Migrations tester", TabControl = "/App_Plugins/packagemigrationtester/views/packagemigrationtester.html" };
        }
    }
}