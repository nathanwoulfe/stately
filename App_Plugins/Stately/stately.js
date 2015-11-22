angular.module('umbraco').controller('stately.ViewController', ['$scope', 'statelyResources', 'dialogService', 'notificationsService', 'navigationService', function ($scope, statelyResources, dialogService, notificationsService, navigationService) {

    // grab the config data
    statelyResources.getall().then(function (resp) {
        $scope.data = resp.data;
    });

    // launches the Umbraco iconpicker dialog
    // callback splits the response into class and colour
    $scope.changeIcon = function (i) {
        var dialog = dialogService.iconPicker({
            callback: function (data) {
                $scope.data[i].CssClass = data.split(' ')[0];
                $scope.data[i].CssColor = data.split(' ')[1];
            }
        });
    }

    /***** sorting *****/
    //defines the options for the jquery sortable    
    $scope.sort = { disabled: true };

    // called on button click, toggles page back to 1 and page length to show all items, or sets to 5
    $scope.enableSorting = function () {
        $scope.sort.disabled = !$scope.sort.disabled;       
    }

    // add a new stately config item
    $scope.addRow = function () {
        var o = {};
        o.CssClass = "icon-smiley";
        o.PropertAlias = "";
        o.Value = true;
        o.Disabled = false;

        $scope.data.push(o);
    }

    // toggles disabled property on row
    $scope.disableRow = function (i) {
        var v = $scope.data[i];
        v.Disabled = v.Disabled === undefined || v.Disabled === false ? true : false;
    };

    // remove a stately row
    $scope.removeRow = function (i) {
        if ($scope.data.length > 1) {
            if (confirm('Are you sure?')) {
                $scope.data.splice(i, 1);
            }
        }
    }

    // save changes back to config
    $scope.saveSettings = function () {
        statelyResources.save($scope.data).then(function (resp) {
            if (resp.data === 'true') {
                notificationsService.success("Success", "Settings have been saved - reload the backoffice to see changes");
                navigationService.reloadSection('content');
            }
            else {
                notificationsService.error("Something went wrong!", "Yeah, that's an error");
            }
        });
    }
}]);

// no more propagation on delete/disable clicks
angular.module('umbraco').directive('stopEvent', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attr) {
            element.bind('click', function (e) {
                e.stopPropagation();
            });
        }
    };
});

// get/post settings back to settings.config file
angular.module("umbraco.resources")
    .factory("statelyResources", function ($http, $cookieStore) {
        return {          

            getall: function () {
                return $http.get("backoffice/stately/SettingsApi/GetSettings");
            },

            save: function (settings) {
                return $http.post("backoffice/stately/SettingsApi/PostSettings", angular.toJson(settings));
            }

        };
    });
