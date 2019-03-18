angular.module("umbraco")
    .controller("packageMigrationTester.Controller",
    function ($scope, $routeParams, notificationsService, packageMigrationTesterResources) {

        $scope.bindData = function () {
            packageMigrationTesterResources.initialize().then(function (res) {
                $scope.loaded = true;
                $scope.packages = res.data;
                $scope.selectedPackage = $scope.packages[0];
                $scope.showMigrations();
            });
        };

        $scope.showMigrations = function () {
            packageMigrationTesterResources.getMigrations($scope.selectedPackage).then(function (res) {
                $scope.migrations = res.data;
            });
        };

        $scope.migrate = function (migration) {
            packageMigrationTesterResources.migrate(migration).then(function (res) {
                notificationsService.success('Migration', 'Package Migration executed succesfully');
                },
                function (data) {
                    notificationsService.error('Error', 'Error occured during install');
                });
        };


        $scope.bindData();
    });