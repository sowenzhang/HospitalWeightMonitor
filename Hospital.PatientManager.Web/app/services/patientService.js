(function () {
    'use strict';

    angular
        .module('app.services')
        .service('PatientService', patientService);

    patientService.$inject = ['$http', '$q', 'baseUrl'];

    function patientService($http, $q, baseUrl) {

        var serviceUrl = baseUrl + "patient/";
        var service = {
            // properties

            // methods
            add: add,
            addWeight: addWeight,
            updateName: updateName,
            removeWeight: removeWeight,
            removePatient: removePatient,

            getAll: getAll,
            get:get,
            getWeightHistory: getWeightHistory
        };

        return service;

        function add(patient) {
            return $http.post(serviceUrl, patient);
        }

        function addWeight(patientId, weight, recordDate, notes) {
            var data = {
                "PatientId": patientId,
                "WeightInKg": weight,
                "RecordDate": recordDate,
                "Notes": notes
            };
            return $http.post(serviceUrl + patientId + "/weight", data);
        }

        function updateName(patientId, firstName, lastName) {
            var data = {
                "PatientId": patientId,
                "FirstName": firstName,
                "LastName": lastName
            };

            return $http.put(serviceUrl + patientId + "/name", data);
        }

        function removeWeight(weightId) {
            return $http.delete(serviceUrl + "weight/" + weightId);
        }

        function removePatient(patientId) {
            return $http.delete(serviceUrl + patientId);
        }

        function getAll() {
            return $http.get(serviceUrl);
        }

        function get(id) {
            return $http.get(serviceUrl + id);
        }

        function getWeightHistory(patientId) {
            return $http.get(serviceUrl + patientId + "/weight");
        }
    }
})();