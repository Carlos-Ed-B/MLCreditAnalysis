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
//# sourceMappingURL=Helper.js.map