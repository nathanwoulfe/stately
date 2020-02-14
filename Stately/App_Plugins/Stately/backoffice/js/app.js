(() => {

    angular.module('stately.services', []);
    angular.module('stately.directives', []);

    angular.module('stately', [
        'stately.services',
        'stately.directives'
    ])
    
    .config($httpProvider => $httpProvider.interceptors.push('statelyInterceptor'))

    //.config(['$provide', $provide => {
    //    $provide.decorator("$rootScope", function ($delegate) {
    //        var Scope = $delegate.constructor;
    //        var origBroadcast = Scope.prototype.$broadcast;
    //        var origEmit = Scope.prototype.$emit;

    //        Scope.prototype.$broadcast = function () {
    //            console.log("$broadcast was called on $scope " + Scope.$id + " with arguments:", arguments);
    //            return origBroadcast.apply(this, arguments);
    //        };
    //        Scope.prototype.$emit = function () {
    //            console.log("$emit was called on $scope " + Scope.$id + " with arguments:", arguments);
    //            return origEmit.apply(this, arguments);
    //        };
    //        return $delegate;
    //    });
    //}]); 


    angular.module('umbraco').requires.push('stately');
})();