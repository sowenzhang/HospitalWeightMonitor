(function() {
    'use strict';

    angular
        .module('app.patient')
        .controller('PatientController', patientController);

    patientController.$inject = ['PatientService', '$mdSidenav', '$mdDialog', '$mdToast'];

    function patientController(patientService, $mdSidenav, $mdDialog, $mdToast) {

        var vm = this;

        // properties
        vm.showSearch = false;
        vm.patients = [];
        vm.message = "";

        // events
        vm.toggleSidenav = toggleSidenav;
        vm.showAdd = showAdd;
        vm.showUpdate = showUpdate;
        vm.showAddWeight = showAddWeight;
        vm.deletePatient = deletePatient;

        // intialize
        activate();

        // handlers
        function toggleSidenav(menuId) {
            $mdSidenav(menuId).toggle();
        };

        function showAdd(ev) {
            $mdDialog.show({
                    controller: "PatientNameController",
                    controllerAs: 'vm',
                    templateUrl: 'html/patient/patient.name.html',
                    targetEvent: ev,
                    locals: {
                        currentPatient: null
                    }
                })
                .then(function () {
                    // TODO:we don't have to reload, the create service actually return the new entity 
                    loadPatients();

                });
        }

        function showAddWeight(item, ev) {
            $mdDialog.show({
                controller: "PatientWeightController",
                controllerAs: 'vm',
                templateUrl: 'html/patient/patient.weight.html',
                targetEvent: ev,
                locals: {
                    currentPatient: item
                }
            }).then(function () {
                // TODO: no need to reload either, only for this project
                loadPatients();
            });
        }

        function showUpdate(item, ev) {
            $mdDialog.show({
                    controller: "PatientNameController",
                    controllerAs: 'vm',
                    locals: {
                        currentPatient: item
                    },
                    templateUrl: 'html/patient/patient.name.html',
                    targetEvent: ev,
                })
                .then(function () {
                    // TODO: no need to reload either, only for this project
                    loadPatients();
                });
        }

        function deletePatient(item) {
            patientService.removePatient(item.Id).then(function () {
                // TODO: no need to reload either, we can simply remove it from the local collection
                loadPatients();
            }, function(err) {
                alert(err);
            });
        }

        // helpers
        function activate() {
            loadPatients();
        }

        function loadPatients() {
            patientService.getAll().then(function (response) {
                vm.patients = [];
                vm.patients = response.data.Items;
            });
        }

        function alert(err) {
            window.helper.handleError(err, vm);
        }
    }
})();