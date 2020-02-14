(() => {

    function statelyService(umbRequestHelper, $http) {

        const urlBase = Umbraco.Sys.ServerVariables.Stately.ApiPath;

        const request = (method, url, data) =>
            umbRequestHelper.resourcePromise(method === 'GET' ? $http.get(url) : $http.post(url, data), 'Something broke');

        return {
            get: () => request('GET', `${urlBase}get`),
            save: settings => request('POST', `${urlBase}post`, settings),
            getAliases: () => request('GET', `${urlBase}getAliases`)
        };
    }

    angular.module('stately.services')
        .service('statelyService', ['umbRequestHelper', '$http', statelyService]);


    function interceptor() {
        return {
            response: resp => {
                if (resp.config.url.indexOf('/content/edit.html') !== -1) {
                    resp.data = resp.data.replace('data-element="editor-content"', 'stately-hook data-element="editor-content"');
                }
                return resp;
            },
        };
    }

    angular.module('stately').factory('statelyInterceptor', interceptor);
})();
