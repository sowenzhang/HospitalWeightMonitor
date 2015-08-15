(function () {
    'use strict';

    angular.module('app.core')
      .run(['$rootScope', '$state', '$stateParams',
            function ($rootScope, $state, $stateParams) {
                $rootScope.$state = $state;
                $rootScope.$stateParams = $stateParams;
            }
      ])
      .config(
        ['$stateProvider', '$urlRouterProvider',
          function ($stateProvider, $urlRouterProvider) {

              $urlRouterProvider
                .otherwise('/home');

              $stateProvider
                .state('app', {
                    "abstract": true,
                    url: '',
                    views: {
                        'app': {
                            templateUrl: 'html/layout/shell.html',
                            controller: 'ShellController',
                            controllerAs: 'vm'
                        }
                    }
                })
                .state('app.home', {
                    url: '/home',
                    views: {
                        'content@app': {
                            templateUrl: 'html/patient/home.html',
                            controller: 'PatientController',
                            controllerAs: 'vm'
                        }
                    }
                })
              .state('app.home.patient', {
                  url: '/patient/:id',
                  views: {
                      'content@app': {
                          templateUrl: 'html/patient/detail.html',
                          controller: 'PatientDetailController',
                          controllerAs: 'vm'
                      }
                  }
              });
          }]
      ).config(function ($mdThemingProvider) {
        var customBlueMap = $mdThemingProvider.extendPalette('light-blue', {
            'contrastDefaultColor': 'light',
            'contrastDarkColors': ['50'],
            '50': 'ffffff'
        });

        $mdThemingProvider.definePalette('customBlue', customBlueMap);
        $mdThemingProvider.theme('default')
          .primaryPalette('customBlue', {
              'default': '500',
              'hue-1': '50'
          }).accentPalette('pink');

        $mdThemingProvider.theme('input', 'default').primaryPalette('grey');
    });

})();
