(function () {
    'use strict';

    angular
        .module('app.patient')
        .controller('PatientWeightController', patientWeightController);

    patientWeightController.$inject = ['PatientService', '$mdDialog', 'currentPatient'];

    function patientWeightController(patientService, $mdDialog, currentPatient) {

        var vm = this;

        // properties
        vm.patient = {
            weight: 60,
            recordDate: new Date(),
            notes: ""
        }; // saving entity
        vm.currentPatient = currentPatient; // passing in entity
        vm.title = "Record Patient Weight";
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
            patientService.addWeight(vm.currentPatient.Id, vm.patient.weight, vm.patient.recordDate, vm.patient.notes).then(function () {
                complete();
            }, function (err) { alert(err); });

            $mdDialog.hide(true);
        }

        // helpers
        function activate() {
            if (currentPatient) {
                vm.patient.weight = currentPatient.WeightInKg;
                vm.patient.recordDate = currentPatient.LastRecordDate;

                if (vm.patient.weight <= 0) vm.patient.weight = 60;
                if (!vm.patient.recordDate) vm.patient.recordDate = new Date();
            } else {
                vm.message = "No patient is passed in.";
            }
        }

        function complete() {
            $mdDialog.hide(true);
        }

        function alert(err) {
            window.helper.handleError(err, vm);
        }
    }
})();