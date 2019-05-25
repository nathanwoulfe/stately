(function () {

    function controller(statelyService, editorService, notificationsService) {

        // grab the config data
        statelyService.get().then(resp => {
            this.data = resp.data || [];
        });

        // launches the Umbraco iconpicker dialog
        // callback splits the response into class and colour
        this.changeIcon = i => {
            const iconPicker = {
                submit: model => {
                    this.data[i].cssClass = model.icon;
                    this.data[i].cssColor = model.color || '';

                    editorService.close();
                },
                close: () => editorService.close()
            };

            editorService.iconPicker(iconPicker);
        };

        /***** sorting *****/
        //defines the options for the jquery sortable    
        this.sort = { disabled: true };

        // called on button click, toggles page back to 1 and page length to show all items, or sets to 5
        this.enableSorting = () => {
            this.sort.disabled = !this.sort.disabled;
        };

        // add a new stately config item
        this.addRow = () => {
            const o = {
                cssClass: 'icon-power',
                cssColor: 'color-black',
                propertyAlias: '',
                value: true,
                disabled: false,
            };

            this.data.push(o);
        };

        // toggles disabled property on row
        this.disableRow = i => {
            const v = this.data[i];
            v.disabled = !v.disabled;
        };

        // remove a stately row
        this.removeRow = i => {
            if (this.data.length) {
                if (confirm('Are you sure?')) {
                    this.data.splice(i, 1);
                }
            }   
        };

        // save changes back to config
        this.saveSettings = () => {
            statelyService.save(this.data).then(resp => {
                if (resp.data === 'true') {
                    notificationsService.success('Success', 'Settings have been saved - reload the backoffice to see changes');
                }
                else {
                    notificationsService.error('Something went wrong!', 'Yeah, that\'s an error');
                }
            });
        };
    }

    angular.module('umbraco').controller('stately.controller',
        ['statelyService', 'editorService', 'notificationsService', controller]);


    // get/post settings back to settings.config file
    angular.module('umbraco.resources')
        .factory('statelyService', function (umbRequestHelper, $http) {

            const request = (method, url, data) =>
                umbRequestHelper.resourcePromise(method === 'GET' ? $http.get(url) : $http.post(url, data), 'Something broke');

            return {
                get: () => request('GET', 'backoffice/stately/api/get'),
                save: settings => request('POST', 'backoffice/stately/api/set', settings)  
            };
        });
}());