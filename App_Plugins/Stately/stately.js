﻿angular.module('umbraco').controller('stately.ViewController', function ($scope, statelyResources, dialogService, notificationsService, navigationService) {

    statelyResources.getall().then(function (resp) {
        $scope.data = resp.data;
    });

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

    $scope.removeRow = function (i) {
        if ($scope.data.length > 1) {
            if (confirm('Are you sure?')) {
                $scope.data.splice(i, 1);
            }
        }
    }

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


});


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


angular.module("umbraco.resources")
    .factory("statelyResources", function ($http, $cookieStore) {
        return {          

            getall: function () {
                return $http.get(this.getApiPath() + "stately/SettingsApi/GetSettings");
            },

            save: function (settings) {
                return $http.post(this.getApiPath() + "stately/SettingsApi/PostSettings", angular.toJson(settings));
            },

            getApiPath: function () {
                var path = $cookieStore.get("statelyUmbracoVersion");
                if (path == null) {
                    try {
                        var version = $http.get("backoffice/stately/SettingsApi/GetUmbracoVersion");
                        path = "backoffice/";
                    } catch (err) {
                        path = "";
                    }

                }
                $cookieStore.put('statelyUmbracoVersion', path);
                return path;
            }
        };
    });