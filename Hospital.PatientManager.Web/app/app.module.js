(function () {
    'use strict';

    var app = angular.module('app', [
        // general logic
        'app.core',

        // framework
        'app.services',
        'app.widgets',

        // features
        'app.shell',
        'app.patient'
    ]);

    app.config(function ($httpProvider) {
        // skip
    });

})();