angular.module('umbraco').controller('stately.ViewController', function ($scope, statelyResources, dialogService, notificationsService, navigationService) {

    statelyResources.getall().then(function (resp) {
        $scope.data = resp.data;
        console.log($scope.data);
    });

    $scope.changeIcon = function (i) {

        var dialog = dialogService.iconPicker({
            callback: function (data) {
                $scope.data[i].CssClass = data.split(' ')[0];
                $scope.data[i].CssColor = data.split(' ')[1];

                console.log($scope.data);
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


angular.module('umbraco').controller('stately.IconController', function ($scope) {

    $scope.icons = ["icon-zoom-out", "icon-truck", "icon-zoom-in", "icon-zip", "icon-axis-rotation", "icon-yen-bag", "icon-axis-rotation-2", "icon-axis-rotation-3", "icon-wrench", "icon-wine-glass", "icon-wrong", "icon-windows", "icon-window-sizes", "icon-window-popin", "icon-wifi", "icon-width", "icon-weight", "icon-war", "icon-wand", "icon-wallet", "icon-wall-plug", "icon-voice", "icon-video", "icon-vcard", "icon-utilities", "icon-users", "icon-users-alt", "icon-user", "icon-user-glasses", "icon-user-females", "icon-user-females-alt", "icon-user-female", "icon-usb", "icon-usb-connector", "icon-unlocked", "icon-universal", "icon-undo", "icon-umbrella", "icon-tv", "icon-tv-old", "icon-trophy", "icon-tree", "icon-trash", "icon-trash-alt", "icon-trash-alt-2", "icon-train", "icon-trafic", "icon-traffic-alt", "icon-top", "icon-tools", "icon-timer", "icon-time", "icon-t-shirt", "icon-tab-key", "icon-tab", "icon-tactics", "icon-tag", "icon-tags", "icon-takeaway-cup", "icon-target", "icon-temperatrure-alt", "icon-temperature", "icon-terminal", "icon-theater", "icon-theif", "icon-thought-bubble", "icon-thumb-down", "icon-thumb-up", "icon-thumbnail-list", "icon-thumbnails-small", "icon-thumbnails", "icon-ticket", "icon-sync", "icon-sweatshirt", "icon-sunny", "icon-stream", "icon-store", "icon-stop", "icon-stop-hand", "icon-stop-alt", "icon-stamp", "icon-stacked-disks", "icon-ssd", "icon-squiggly-line", "icon-sprout", "icon-split", "icon-split-alt", "icon-speed-gauge", "icon-speaker", "icon-sound", "icon-spades", "icon-sound-waves", "icon-shipping-box", "icon-shipping", "icon-shoe", "icon-shopping-basket-alt-2", "icon-shopping-basket", "icon-shopping-basket-alt", "icon-shorts", "icon-shuffle", "icon-sience", "icon-simcard", "icon-single-note", "icon-sitemap", "icon-sleep", "icon-slideshow", "icon-smiley-inverted", "icon-smiley", "icon-snow", "icon-sound-low", "icon-sound-medium", "icon-sound-off", "icon-shift", "icon-shield", "icon-sharing-iphone", "icon-share", "icon-share-alt", "icon-share-alt-2", "icon-settings", "icon-settings-alt", "icon-settings-alt-2", "icon-server", "icon-server-alt", "icon-sensor", "icon-security-camera", "icon-search", "icon-scull", "icon-script", "icon-script-alt", "icon-screensharing", "icon-school", "icon-scan", "icon-refresh", "icon-remote", "icon-remove", "icon-repeat-one", "icon-repeat", "icon-resize", "icon-reply-arrow", "icon-return-to-top", "icon-right-double-arrow", "icon-road", "icon-roadsign", "icon-rocket", "icon-rss", "icon-ruler-alt", "icon-ruler", "icon-sandbox-toys", "icon-satellite-dish", "icon-save", "icon-safedial", "icon-safe", "icon-redo", "icon-printer-alt", "icon-planet", "icon-paste-in", "icon-os-x", "icon-navigation-left", "icon-message", "icon-lock", "icon-layers-alt", "icon-record", "icon-print", "icon-plane", "icon-partly-cloudy", "icon-ordered-list", "icon-navigation-last", "icon-message-unopened", "icon-location-nearby", "icon-laptop", "icon-reception", "icon-price-yen", "icon-piracy", "icon-parental-control", "icon-operator", "icon-navigation-horizontal", "icon-message-open", "icon-lab", "icon-location-near-me", "icon-receipt-yen", "icon-price-pound", "icon-pin-location", "icon-parachute-drop", "icon-old-phone", "icon-merge", "icon-navigation-first", "icon-locate", "icon-keyhole", "icon-receipt-pound", "icon-price-euro", "icon-piggy-bank", "icon-paper-plane", "icon-old-key", "icon-navigation-down", "icon-megaphone", "icon-loading", "icon-keychain", "icon-receipt-euro", "icon-price-dollar", "icon-pie-chart", "icon-paper-plane-alt", "icon-notepad", "icon-navigation-bottom", "icon-meeting", "icon-keyboard", "icon-load", "icon-receipt-dollar", "icon-previous", "icon-pictures", "icon-notepad-alt", "icon-paper-bag", "icon-name-badge", "icon-medicine", "icon-list", "icon-key", "icon-receipt-alt", "icon-previous-media", "icon-pictures-alt", "icon-pants", "icon-nodes", "icon-music", "icon-readonly", "icon-presentation", "icon-pictures-alt-2", "icon-pannel-close", "icon-next", "icon-multiple-windows", "icon-medical-emergency", "icon-medal", "icon-link", "icon-linux-tux", "icon-junk", "icon-item-arrangement", "icon-iphone", "icon-lightning", "icon-map", "icon-multiple-credit-cards", "icon-next-media", "icon-panel-show", "icon-picture", "icon-power", "icon-re-post", "icon-rate", "icon-rain", "icon-radio", "icon-radio-receiver", "icon-radio-alt", "icon-quote", "icon-qr-code", "icon-pushpin", "icon-pulse", "icon-projector", "icon-play", "icon-playing-cards", "icon-playlist", "icon-plugin", "icon-podcast", "icon-poker-chip", "icon-poll", "icon-post-it", "icon-pound-bag", "icon-power-outlet", "icon-photo-album", "icon-phone", "icon-phone-ring", "icon-people", "icon-people-female", "icon-people-alt", "icon-people-alt-2", "icon-pc", "icon-pause", "icon-path", "icon-out", "icon-outbox", "icon-outdent", "icon-page-add", "icon-page-down", "icon-page-remove", "icon-page-restricted", "icon-page-up", "icon-paint-roller", "icon-palette", "icon-newspaper", "icon-newspaper-alt", "icon-network-alt", "icon-navigational-arrow", "icon-navigation", "icon-navigation-vertical", "icon-navigation-up", "icon-navigation-top", "icon-navigation-road", "icon-navigation-right", "icon-microscope", "icon-mindmap", "icon-molecular-network", "icon-molecular", "icon-mountain", "icon-mouse-cursor", "icon-mouse", "icon-movie-alt", "icon-map-marker", "icon-movie", "icon-map-location", "icon-map-alt", "icon-male-symbol", "icon-male-and-female", "icon-mailbox", "icon-magnet", "icon-loupe", "icon-mobile", "icon-logout", "icon-log-out", "icon-layers", "icon-left-double-arrow", "icon-layout", "icon-legal", "icon-lense", "icon-library", "icon-light-down", "icon-light-up", "icon-lightbulb-active", "icon-lightbulb", "icon-ipad", "icon-invoice", "icon-info", "icon-infinity", "icon-indent", "icon-inbox", "icon-inbox-full", "icon-inactive-line", "icon-imac", "icon-hourglass", "icon-home", "icon-grid", "icon-food", "icon-favorite", "icon-door-open-alt", "icon-diagnostics", "icon-contrast", "icon-coins-dollar-alt", "icon-circle-dotted-active", "icon-cinema", "icon-chip", "icon-chip-alt", "icon-chess", "icon-checkbox", "icon-checkbox-empty", "icon-checkbox-dotted", "icon-checkbox-dotted-active", "icon-check", "icon-chat", "icon-chat-active", "icon-chart", "icon-chart-curve", "icon-certificate", "icon-categories", "icon-cash-register", "icon-car", "icon-caps-lock", "icon-candy", "icon-circle-dotted", "icon-circuits", "icon-circus", "icon-client", "icon-clothes-hanger", "icon-cloud-drive", "icon-cloud-upload", "icon-cloud", "icon-cloudy", "icon-clubs", "icon-cocktail", "icon-code", "icon-coffee", "icon-coin-dollar", "icon-coin-pound", "icon-coin-yen", "icon-coin", "icon-coins-alt", "icon-console", "icon-connection", "icon-compress", "icon-company", "icon-command", "icon-coin-euro", "icon-combination-lock", "icon-combination-lock-open", "icon-comb", "icon-columns", "icon-colorpicker", "icon-color-bucket", "icon-coins", "icon-coins-yen", "icon-coins-yen-alt", "icon-coins-pound", "icon-coins-pound-alt", "icon-coins-euro", "icon-coins-euro-alt", "icon-coins-dollar", "icon-conversation-alt", "icon-conversation", "icon-coverflow", "icon-credit-card-alt", "icon-credit-card", "icon-crop", "icon-crosshair", "icon-crown-alt", "icon-crown", "icon-cupcake", "icon-curve", "icon-cut", "icon-dashboard", "icon-defrag", "icon-delete", "icon-delete-key", "icon-departure", "icon-desk", "icon-desktop", "icon-donate", "icon-dollar-bag", "icon-documents", "icon-document", "icon-document-dashed-line", "icon-dock-connector", "icon-dna", "icon-display", "icon-disk-image", "icon-disc", "icon-directions", "icon-directions-alt", "icon-diploma", "icon-diploma-alt", "icon-dice", "icon-diamonds", "icon-diamond", "icon-diagonal-arrow", "icon-diagonal-arrow-alt", "icon-door-open", "icon-download-alt", "icon-download", "icon-drop", "icon-eco", "icon-economy", "icon-edit", "icon-eject", "icon-employee", "icon-energy-saving-bulb", "icon-enter", "icon-equalizer", "icon-escape", "icon-ethernet", "icon-euro-bag", "icon-exit-fullscreen", "icon-eye", "icon-facebook-like", "icon-factory", "icon-font", "icon-folders", "icon-folder", "icon-folder-close", "icon-folder-outline", "icon-folder-open", "icon-flowerpot", "icon-flashlight", "icon-flash", "icon-flag", "icon-flag-alt", "icon-firewire", "icon-firewall", "icon-fire", "icon-fingerprint", "icon-filter", "icon-filter-arrows", "icon-files", "icon-file-cabinet", "icon-female-symbol", "icon-footprints", "icon-hammer", "icon-hand-active-alt", "icon-forking", "icon-hand-active", "icon-hand-pointer-alt", "icon-hand-pointer", "icon-handprint", "icon-handshake", "icon-handtool", "icon-hard-drive", "icon-help", "icon-graduate", "icon-gps", "icon-help-alt", "icon-height", "icon-globe", "icon-hearts", "icon-globe-inverted-europe-africa", "icon-headset", "icon-globe-inverted-asia", "icon-headphones", "icon-globe-inverted-america", "icon-hd", "icon-globe-europe---africa", "icon-hat", "icon-globe-asia", "icon-globe-alt", "icon-hard-drive-alt", "icon-glasses", "icon-gift", "icon-handtool-alt", "icon-geometry", "icon-game", "icon-fullscreen", "icon-fullscreen-alt", "icon-frame", "icon-frame-alt", "icon-camera-roll", "icon-bookmark", "icon-bill", "icon-baby-stroller", "icon-alarm-clock", "icon-adressbook", "icon-add", "icon-activity", "icon-untitled", "icon-glasses", "icon-camcorder", "icon-calendar", "icon-calendar-alt", "icon-calculator", "icon-bus", "icon-burn", "icon-bulleted-list", "icon-bug", "icon-brush", "icon-brush-alt", "icon-brush-alt-2", "icon-browser-window", "icon-briefcase", "icon-brick", "icon-brackets", "icon-box", "icon-box-open", "icon-box-alt", "icon-books", "icon-billboard", "icon-bills-dollar", "icon-bills-euro", "icon-bills-pound", "icon-bills-yen", "icon-bills", "icon-binarycode", "icon-binoculars", "icon-bird", "icon-birthday-cake", "icon-blueprint", "icon-block", "icon-bluetooth", "icon-boat-shipping", "icon-bomb", "icon-book-alt-2", "icon-bones", "icon-book-alt", "icon-book", "icon-bill-yen", "icon-award", "icon-bill-pound", "icon-autofill", "icon-bill-euro", "icon-auction-hammer", "icon-bill-dollar", "icon-attachment", "icon-bell", "icon-article", "icon-bell-off", "icon-art-easel", "icon-beer-glass", "icon-arrow-up", "icon-battery-low", "icon-arrow-right", "icon-battery-full", "icon-arrow-left", "icon-bars", "icon-arrow-down", "icon-barcode", "icon-arrivals", "icon-bar-chart", "icon-application-window", "icon-band-aid", "icon-application-window-alt", "icon-ball", "icon-application-error", "icon-badge-restricted", "icon-app", "icon-badge-remove", "icon-anchor", "icon-badge-count", "icon-alt", "icon-badge-add", "icon-alert", "icon-backspace", "icon-alert-alt"];

    $scope.setIcon = function (icon) {
        $scope.submit(icon);
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
                var path = $cookieStore.get("PieManUmbracoVersion");
                if (path == null) {
                    try {
                        var version = $http.get("backoffice/stately/SettingsApi/GetUmbracoVersion");
                        path = "backoffice/";
                    } catch (err) {
                        path = "";
                    }

                }
                $cookieStore.put('PieManUmbracoVersion', path);
                return path;
            }
        };
    });
