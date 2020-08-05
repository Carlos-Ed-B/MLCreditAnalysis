function onSendBegin() {
    //alert('onSendBegin');
    //debugger;
    console.log('onSendBegin');
}

function onSendComplete(result) {
    console.log(result);
    console.log(result.responseJSON);

    $results = $("#formResults");
    $btnDoAction = $("#btnSendxx");

    NotifyHelper.showMessage(result.responseJSON);
    Helper.removeLoading($results, $btnDoAction);
    //$('#formToAction').trigger("reset");

    //alert('onSendComplete');
    //debugger;
}

function onSendSucess() {
    console.log('onSendSucess');
    //alert('onSendSucess');
    //debugger;
}

function onSendFailure() {
    console.log('onSendFailure');
    //alert('onSendFailure');
    //debugger;
}
