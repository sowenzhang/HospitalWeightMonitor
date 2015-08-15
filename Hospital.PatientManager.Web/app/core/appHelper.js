// some javascript helper methods for this app specific 
function AppHelper() {

    var instance = this;

    instance.handleError = handleError;
    instance.formatDate = formatDate;

    return instance;

    // here is trying to have a generic error handler for the entire application
    // err - the error response from the service, required 
    // vm - the viewmodel, required 
    // msgPrefix - optional, we prefix the error message using it
    function handleError(err, vm, msgPrefix, status) {

        if (!vm) return;

        if (status === 404) {
            // TODO: go to a specific page
        }

        if (err != null) {
            var errors = [];
            if (err.ModelState) {
                for (var key in err.ModelState) {
                    for (var i = 0; i < response.data.ModelState[key].length; i++) {
                        errors.push(response.data.ModelState[key][i]);
                    }
                }

                vm.message = (msgPrefix || '') + errors.join(' ');
            } else {
                if (err.ExceptionMessage !== undefined)
                    vm.message = (msgPrefix || '') + err.ExceptionMessage;
                else if (err.error_description !== undefined)
                    vm.message = (msgPrefix || '') + err.error_description;
                else if (err.data) {
                    if (err.data.ExceptionMessage !== undefined) {
                        vm.message = (msgPrefix || '') + err.data.ExceptionMessage;
                    } else {
                        vm.message = (msgPrefix || '') + err.data.Message;
                    }
                } else if (err.substring)
                    vm.message = (msgPrefix || '') + err;
                else
                    vm.message = (msgPrefix || '') + "unknown error, please try again later.";
            }
        } else {
            vm.message = "An error just occurred, please try again later.";
        }
    }

    // the input should be a datetime offset format
    function formatDate(input) {
        var date = new Date(input);
        var retval = date.toJSON();
        retval = retval.slice(0, 10);
        return retval;
    }

};

window.helper = new AppHelper();