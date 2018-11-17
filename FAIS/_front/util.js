String.prototype.replaceAll = function (searchStr, replaceStr) {
    var str = this;

    // escape regexp special characters in search string
    searchStr = searchStr.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');

    return str.replace(new RegExp(searchStr, 'gi'), replaceStr);
};
function clone(src) {
    return Object.assign({}, src);
}

jQuery.fn.apiload = function (url, params, callback) {
    var selector, type, response,
        self = this,
        off = url.indexOf(" "),
        ajxstngs = {};

    if (off > -1) {
        selector = stripAndCollapse(url.slice(off));
        url = url.slice(0, off);
    }

    // If it's a function
    if (jQuery.isFunction(params)) {

        // We assume that it's the callback
        callback = params;
        params = undefined;

        // Otherwise, build a param string
    } else if (params && typeof params === "object") {
        type = "POST";
    }

    ajxstngs = {
        url: url,

        // If "type" variable is undefined, then "GET" method will be used.
        // Make value of this field explicit since
        // user can override it through ajaxSetup method
        type: type || "GET",
        dataType: "html",
        data: params
    };

    if (typeof headers !== "undefined") ajxstngs.headers = headers;
    // If we have elements to modify, make the request
    if (self.length > 0) {
        jQuery.ajax(ajxstngs).done(function (responseText) {

            // Save response for use in complete callback
            response = arguments;

            self.html(selector ?

                // If a selector was specified, locate the right elements in a dummy div
                // Exclude scripts to avoid IE 'Permission Denied' errors
                jQuery("<div>").append(jQuery.parseHTML(responseText)).find(selector) :

                // Otherwise use the full result
                responseText);

            // If the request succeeds, this function gets "data", "status", "jqXHR"
            // but they are ignored because response was set above.
            // If it fails, this function gets "jqXHR", "status", "error"
        }).always(callback && function (jqXHR, status) {
            self.each(function () {
                callback.apply(this, response || [jqXHR.responseText, status, jqXHR]);
            });
        });
    }

    return this;
};

function updateDom() {
    $.material.init();
    if ($(".selectpicker").length != 0) {
        $(".selectpicker").selectpicker();
    }
    $('.datepicker').datetimepicker({
        format: 'MM/DD/YYYY',
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-chevron-up",
            down: "fa fa-chevron-down",
            previous: 'fa fa-chevron-left',
            next: 'fa fa-chevron-right',
            today: 'fa fa-screenshot',
            clear: 'fa fa-trash',
            close: 'fa fa-remove',
            inline: true
        }
    });

}
var NOTIF = {
    show: function (text, type, icone, from, align) {
        // type = ['rose', 'primary'];
        if (typeof type === "undefined") type = "";
        if (typeof icone === "undefined") icone = "notifications";
        if (typeof from === "undefined") from = "top";
        if (typeof align === "undefined") align = "center";
        $.notify({
            icon: icone,
            message: text

            }, {
                type: type,
                timer: 3000,
                placement: {
                    from: from,
                    align: align
                }
        });
    },
    success: function success(text) {
        this.show(text, 'success');
    },
    info: function success(text) {
        this.show(text, 'info');
    },
    warning: function success(text) {
        this.show(text, 'warning');
    },
    error: function success(text) {
        this.show(text, 'danger');
    }
}

function sideBarFix() {
    if (isWindows) {
        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar('destroy');
        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar();
    }
}