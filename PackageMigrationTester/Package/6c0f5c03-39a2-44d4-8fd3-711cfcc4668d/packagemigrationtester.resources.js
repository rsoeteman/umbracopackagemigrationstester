angular.module("umbraco.resources").factory("packageMigrationTesterResources", function ($http) {

        return {
            initialize: function () {
                return $http.get("backoffice/packagemigrationtester/packagemigrationtesterapi/initialize");
            },
            getMigrations: function (packageName) {
                return $http.get("backoffice/packagemigrationtester/packagemigrationtesterapi/getmigrations", { params: { packageName: packageName } });
            },
            migrate: function (migration) {
                return $http.post("backoffice/packagemigrationtester/packagemigrationtesterapi/migrate", migration);
            }
        };
    });