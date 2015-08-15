(function () {
    'use strict';

    angular.module('app.services', ['ngResource'])
        .constant("baseUrl", 'http://localhost:49733/api/v1/')
        .config([
            '$httpProvider', function ($httpProvider) {
                //Enable cross domain calls
                $httpProvider.defaults.useXDomain = true;

                //Remove the header containing XMLHttpRequest used to identify ajax call 
                //that would prevent CORS from working
                delete $httpProvider.defaults.headers.common['X-Requested-With'];

                $httpProvider.defaults.headers.post = {
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE',
                    'Access-Control-Allow-Headers': 'Content-Type, Accept, X-Requested-With',
                    'Content-Type': 'application/json'
                };

                $httpProvider.defaults.headers.put = {
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE',
                    'Access-Control-Allow-Headers': 'Content-Type, Accept, X-Requested-With',
                    'Content-Type': 'application/json'
                };
            }
        ]);

})();