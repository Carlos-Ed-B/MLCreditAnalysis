var Helper = (function () {
    function Helper() {
    }
    Helper.setLoading = function ($selector, $buttonSelector) {
        $selector.addClass("isLoading");
        $buttonSelector.button('loading');
    };
    ;
    Helper.removeLoading = function ($selector, $buttonSelector) {
        $selector.removeClass("isLoading");
        $buttonSelector.button('reset');
    };
    ;
    Helper.getDateEmpty = function () {
        return new Date('0001-01-01');
    };
    ;
    Helper.setDisable = function (seletorId) {
        $(seletorId).prop('disabled', true);
    };
    ;
    Helper.setEnable = function (seletorId) {
        $(seletorId).prop('disabled', false);
    };
    ;
    Helper.isEmpty = function (value) {
        return value === "" || value === undefined;
    };
    ;
    Helper.isListEmpty = function (value) {
        return Helper.isEmpty(value) && value.length === 0;
    };
    ;
    Helper.isIntegerEmpty = function (value) {
        return value === "" || value === "0" || value === undefined;
    };
    ;
    Helper.isDataEmpty = function (value) {
        return value === "" || value === undefined || value === "__/__/____" || new Date(value).getFullYear() === 0;
    };
    ;
    Helper.hasListValue = function (value) {
        return !Helper.isListEmpty(value);
    };
    ;
    Helper.convertDatePTtoDateYYYmmdd = function (value) {
        var re = /^\d{1,2}\/\d{1,2}\/\d{4}$/;
        if (re.test(value)) {
            var adata = value.split('/');
            var gg = parseInt(adata[0], 10);
            var mm = parseInt(adata[1], 10);
            var aaaa = parseInt(adata[2], 10);
            var dataBr = new Date(aaaa, mm - 1, gg);
            if ((dataBr.getFullYear() == aaaa) && (dataBr.getMonth() == mm - 1) && (dataBr.getDate() == gg)) {
                return dataBr;
            }
        }
        return Helper.getDateEmpty();
    };
    Helper.clearMaskOn = function (e) {
        if (46 == e.keyCode || 8 == e.keyCode || 9 == e.keyCode) {
            var $this = $(this);
            if ($this.val() == "__/__/____")
                $this.val("");
        }
    };
    Helper.addDatepicker = function (seletorId) {
        $(seletorId).datepicker().mask("99/99/9999");
    };
    Helper.setVisible = function (seletorId) {
        $(seletorId).css("visibility", "visible");
    };
    Helper.setInvisible = function (seletorId) {
        $(seletorId).css("visibility", "hidden");
    };
    return Helper;
}());

//function loadCamera() {
//    document.querySelector(".area-do-video").style.display = "block";
//    //Captura elemento de vídeo
//    var video = document.querySelector("#webCamera");
//    //As opções abaixo são necessárias para o funcionamento correto no iOS
//    video.setAttribute('autoplay', '');
//    video.setAttribute('muted', '');
//    video.setAttribute('playsinline', '');
//    //--

//    //Verifica se o navegador pode capturar mídia
//    if (navigator.mediaDevices.getUserMedia) {
//        navigator.mediaDevices.getUserMedia({ audio: false, video: { facingMode: 'user' } })
//            .then(function (stream) {
//                //Definir o elemento víde a carregar o capturado pela webcam
//                video.srcObject = stream;
//            })
//            .catch(function (error) {
//                alert("Oooopps... Falhou :'(");
//            });
//    }
//}

//function takeSnapShot() {
//    //Captura elemento de vídeo
//    var video = document.querySelector("#webCamera");

//    //Criando um canvas que vai guardar a imagem temporariamente
//    var canvas = document.createElement('canvas');
//    canvas.width = video.videoWidth;
//    canvas.height = video.videoHeight;
//    var ctx = canvas.getContext('2d');

//    //Desnehando e convertendo as minensões
//    ctx.drawImage(video, 0, 0, canvas.width, canvas.height);

//    //Criando o JPG
//    var dataURI = canvas.toDataURL('image/jpeg'); //O resultado é um BASE64 de uma imagem.
//    document.querySelector("#base_img").value = dataURI;
//    debugger;

//    //sendSnapShot(dataURI); //Gerar Imagem e Salvar Caminho no Banco
//}

//function sendSnapShot(base64) {
//    var request = new XMLHttpRequest();
//    request.open('POST', '/testes/save_photos.php', true);
//    request.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded; charset=UTF-8');

//    request.onload = function () {
//        console.log(request);
//        if (request.status >= 200 && request.status < 400) {
//            //Colocar o caminho da imagem no SRC
//            var data = JSON.parse(request.responseText);

//            //verificar se houve erro
//            if (data.error) {
//                alert(data.error);
//                return false;
//            }

//            //Mostrar informações
//            document.querySelector("#imagemConvertida").setAttribute("src", "/testes/" + data.img);
//            document.querySelector("#caminhoImagem a").setAttribute("href", "/testes/" + data.img);
//            document.querySelector("#caminhoImagem a").innerHTML = data.img.split("/")[1];
//        } else {
//            alert("Erro ao salvar. Tipo:" + request.status);
//        }
//    };

//    request.onerror = function () {
//        alert("Erro ao salvar. Back-End inacessível.");
//    }

//    request.send("base_img=" + base64); // Enviar dados
//}