var NotifyHelper = (function () {

    function NotifyHelper() {
    }

    NotifyHelper.errorValidationSummary = function () {
        var validationSummaryContainer = ".validation-summary-errors";

        if ($(validationSummaryContainer).text() != '') {
            var validationSummaryError = "";
            $(validationSummaryContainer + ' li').each(function () {
                validationSummaryError += $(this).html();
                console.log($(this).html());
            });
            NotifyHelper.error(validationSummaryError);
        }
    };
    NotifyHelper.showMessage = function (userMessage) {

        if (userMessage.typeResult === UserMessageCategoryEnum.Error) {
            NotifyHelper.error(userMessage.message);
            return;
        }
        if (userMessage.typeResult === UserMessageCategoryEnum.Warning) {
            NotifyHelper.warning(userMessage.message);
            return;
        }
        if (userMessage.typeResult === UserMessageCategoryEnum.Message) {
            NotifyHelper.success(userMessage.message);
            return;
        }

        alert(userMessage.description);
    };

    NotifyHelper.success = function (message) {
        NotifyHelper.generic(message, "success");
    };

    NotifyHelper.error = function (message) {
        NotifyHelper.generic(message, "error");
    };

    NotifyHelper.info = function (message) {
        NotifyHelper.generic(message, "info");
    };

    NotifyHelper.warning = function (message) {
        NotifyHelper.generic(message, "Warning");
    };

    NotifyHelper.showMessageSendFailed = function () {
        NotifyHelper.generic("Ocorreu um erro ao efetuar o processamento.", "error");
    };

    NotifyHelper.generic = function (message, classNameMessage) {
        if (message == undefined) { message = ""; }
        $.notify(message, { position: "top-center", className: classNameMessage, hideDuration: 400 });
    };

    return NotifyHelper;
}());