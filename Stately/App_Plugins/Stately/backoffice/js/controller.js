(() => {

    function controller($q, statelyService, editorService, notificationsService) {

        $q.all([statelyService.get(), statelyService.getAliases()])
            .then(resp => {
                this.data = resp[0].data || [];
                this.aliases = resp[1].data;
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
        // defines the options for the jquery sortable    
        // pinched from umbraco's nested content doctype selection table implementation
        this.sortableOptions = {
            axis: 'y',
            cursor: 'move',
            handle: '.handle',
            placeholder: 'sortable-placeholder',
            forcePlaceholderSize: true,
            helper: function helper(e, ui) {
                // When sorting table rows, the cells collapse. This helper fixes that: https://www.foliotek.com/devblog/make-table-rows-sortable-using-jquery-ui-sortable/
                ui.children().each(function () {
                    $(this).width($(this).width());
                });
                return ui;
            },
            start: function start(e, ui) {
                var cellHeight = ui.item.height();
                // Build a placeholder cell that spans all the cells in the row: https://stackoverflow.com/questions/25845310/jquery-ui-sortable-and-table-cell-size
                var cellCount = 0;
                $('td, th', ui.helper).each(function () {
                    // For each td or th try and get it's colspan attribute, and add that or 1 to the total
                    var colspan = 1;
                    var colspanAttr = $(this).attr('colspan');
                    if (colspanAttr > 1) {
                        colspan = colspanAttr;
                    }
                    cellCount += colspan;
                });
                // Add the placeholder UI - note that this is the item's content, so td rather than tr - and set height of tr
                ui.placeholder.html('<td colspan="' + cellCount + '"></td>').height(cellHeight);
            }
        };

        // add a new stately config item
        this.addRow = () => {
            const o = {
                cssClass: 'icon-power',
                cssColor: 'color-black',
                propertyAlias: '',
                value: true,
                disabled: false,
                replace: false,
                recolor: false
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
                if (resp.data) {
                    notificationsService.success('Success', 'Settings have been saved - reload the backoffice to see changes');
                }
                else {
                    notificationsService.error('Something went wrong!', 'Yeah, that\'s an error');
                }
            });
        };
    }

    angular.module('stately').controller('stately.controller',
        ['$q', 'statelyService', 'editorService', 'notificationsService', controller]);

})();