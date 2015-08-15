(function () {
    'use strict';

    angular
        .module('app.patient')
        .controller('PatientNameController', patientNameController);

    patientNameController.$inject = ['PatientService', '$mdDialog', 'currentPatient'];

    function patientNameController(patientService, $mdDialog, currentPatient) {

        var vm = this;

        // properties
        vm.patient = { FirstName: "", LastName: "" };
        vm.currentPatient = currentPatient;
        vm.title = "Add Patient";
        vm.message = "";

        // events
        vm.close = close;
        vm.submit = submit;

        // intialize
        activate();

        // handlers
        function close() {
            $mdDialog.cancel();
        }

        function submit() {
            vm.message = "";
            if (vm.currentPatient) {
                updatePatient();
            } else {
                createPatient();
            }
        }

        // helpers
        function activate() {
            if (vm.currentPatient) {
                vm.title = "Update Patient";
                vm.patient.FirstName = vm.currentPatient.FirstName;
                vm.patient.LastName = vm.currentPatient.LastName;
            }
        }

        function updatePatient() {
            patientService.updateName(vm.currentPatient.Id, vm.patient.FirstName, vm.patient.LastName)
                .then(function () {
                    complete();
                }, function (err) {
                    alert(err);
                });
        }

        function createPatient() {
            patientService.add(vm.patient).then(function () {
                complete();
            }, function (err) {
                alert(err);
            });
        }

        function complete() {
            $mdDialog.hide(true);
        }

        function alert(err) {
            window.helper.handleError(err, vm);
        }
    }
})();