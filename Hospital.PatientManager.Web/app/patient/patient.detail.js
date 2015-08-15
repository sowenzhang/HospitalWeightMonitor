(function () {
    'use strict';

    angular
        .module('app.patient')
        .controller('PatientDetailController', patientDetailController);

    patientDetailController.$inject = ['PatientService', '$timeout', '$mdDialog', '$rootScope'];

    function patientDetailController(patientService, $timeout,$mdDialog, $rootScope) {

        var vm = this;

        // properties
        vm.patient = {};
        vm.title = "Patient Detail";
        vm.patientId = $rootScope.$stateParams.id;
        vm.message = "";
        vm.histories = [];
        vm.chart = {
            type: "line", 
            data: {
                series: ['Weight'],
                data: [{x:'', y: [0]}]
            },
            config: {
                title:false,
                labels: false,
                legend: {
                    display: true,
                    position: 'left'
                },
                innerRadius: 0,
                waitForHeightAndWidth: true
            }
        };

        // events
        vm.showAddWeight = showAddWeight;
        vm.formatDate = formatDate;
        vm.removeWeight = removeWeight;

        // intialize
        activate();

        // handlers
        function removeWeight(weight) {
            patientService.removeWeight(weight.Id).then(function() {
                loadHistory();
            }, function(err) {
                window.helper.handleError(err, vm);
            });
        }

        function formatDate(d) {
            if (d) return window.helper.formatDate(d);
            return "";
        }

        function showAddWeight(ev) {
            $mdDialog.show({
                controller: "PatientWeightController",
                controllerAs: 'vm',
                templateUrl: 'html/patient/patient.weight.html',
                targetEvent: ev,
                locals: {
                    currentPatient: vm.patient
                }
            }).then(function () {
                loadHistory();
            });
        }

        // helpers
        function activate() {
            if (vm.patientId) {
                patientService.get(vm.patientId).then(function (response) {
                    vm.patient = response.data;
                    loadHistory();
                }, function(err) {
                    window.helper.handleError(err, vm);
                });
            } else {
                vm.message = "No Patient Id is specified, will direct you to patient list shortly";
                gohome();
            }
        }

        function loadHistory() {
            patientService.getWeightHistory(vm.patientId).then(function (response) {
                vm.histories = [];
                vm.histories = response.data.Items;
                initChart();
            }, function(err) {
                window.helper.handleError(err, vm);
            });
        }

        function initChart() {
            $timeout(function() {
                vm.chart.data.data = [];
                // the data from the service are sorted by date in descending order
                for (var idx = vm.histories.length-1; idx >=0; idx--) {
                    var record = vm.histories[idx];
                    vm.chart.data.data.push({
                        x: vm.formatDate(record.RecordDate),
                        y: [record.WeightInKg]
                    });
                }
            });
        }

        function gohome() {
            var delay = 2000;

            var timer = $timeout(function () {
                $timeout.cancel(timer);
                $rootScope.$state.go('app.home');
            }, delay);
        }
    }
})();