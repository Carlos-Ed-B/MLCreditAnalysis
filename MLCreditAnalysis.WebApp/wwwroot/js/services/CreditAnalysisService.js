function onSendBegin() {
    console.log('onSendBegin');
    addButtonLoading();
}

function onSendComplete(result) {
    //console.log(result);
    console.log(result.responseJSON);
    debugger;

    $results = $("#formResults");
    $btnDoAction = $("#btnSendxx");

    NotifyHelper.showMessage(result.responseJSON);
    Helper.removeLoading($results, $btnDoAction);

    removeButtonLoading();

    if (result.responseJSON.typeResult === 1) {
        document.getElementById('imageResult').setAttribute('src', 'data:image/png;base64,' + result.responseJSON.content.imageFileBase64);
    }
}

function onSendSucess() {
    console.log('onSendSucess');
}

function onSendFailure() {
    console.log('onSendFailure');
}

$(document).ready(function () {
    //$("#my_camera").width(445).height(240);
});

function addButtonLoading() {
    $('#btnDoAction').text('Enviando...').prop('disabled', true);
    $('#btnDoActionLoading').addClass("spinner-grow spinner-grow-sm");
}

function removeButtonLoading() {
    $('#btnDoAction').text('Enviar').prop('disabled', false);
    $('#btnDoActionLoading').removeClass("spinner-grow spinner-grow-sm");
}

