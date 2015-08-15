(function () {
    'use strict';

    angular
        .module('app.shell')
        .controller('ShellController', shellController);

    shellController.$inject = [];

    function shellController() {

        var vm = this;
        vm.menu = [
            {
                link: 'app.home',
                title: 'All Patients',
                icon: 'group'
            }
        ];
    }
})();