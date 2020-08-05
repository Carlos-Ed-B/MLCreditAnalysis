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


function loadMunicipio() {
    var uf = $('#Uf').val();

    if (uf === undefined || uf === "") {
        var options = $("#Municipio");
        options.empty();
        console.log("Limpando municipio....");
        return;
    }

    $.getJSON("GeographicLocationBrazil\\ConsultMunicipio?uf=" + uf, function (result) {
        console.log(result);

        var options = $("#Municipio");
        options.empty();

        options.append($("<option />").val("").text(""));

        $.each(result.content, function (item, value) {
            options.append($("<option />").val(value).text(value));
        });
    });
}

function loadKM() {
    var municipio = $('#Municipio').val();

    if (municipio === undefined || municipio === "") {
        var options = $("#Km");
        options.empty();
        console.log("Limpando km....");
        return;
    }

    $.getJSON("GeographicLocationBrazil\\ConsultKM?municipio=" + municipio, function (result) {
        console.log(result);

        var options = $("#Km");
        options.empty();

        options.append($("<option />").val("").text(""));

        $.each(result.content, function (item, value) {
            options.append($("<option />").val(value).text(value));
        });
    });
}

function loadVehicleBrand() {
    var vehicleType = $('#Tipo_veiculo').val();

    if (vehicleType === undefined || vehicleType === "") {
        var options = $("#Marca");
        options.empty();
        console.log("Limpando marca veiculo....");
        return;
    }

    $.getJSON("Vehicle\\ConsultBrand?vehicleType=" + vehicleType, function (result) {
        console.log(result);

        var options = $("#Marca");
        options.empty();

        options.append($("<option />").val("").text(""));

        $.each(result.content, function (item, value) {
            options.append($("<option />").val(value).text(value));
        });
    });
}

$(document).ready(function () {
    //loadMunicipio();

    $("#Uf").change(function () {
        console.log('Carregando municipio...');
        loadMunicipio();
    });

    $("#Municipio").change(function () {
        console.log('Carregando km...');
        loadKM();
    });

    $("#Tipo_veiculo").change(function () {
        console.log('Carregando marca veiculo...');
        loadVehicleBrand();
    });
});