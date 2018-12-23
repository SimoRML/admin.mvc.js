var baseUrl = "";
String.prototype.replaceAll = function (searchStr, replaceStr) {
    var str = this;

    // escape regexp special characters in search string
    searchStr = searchStr.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');

    return str.replace(new RegExp(searchStr, 'gi'), replaceStr);
};
function clone(src) {
    return Object.assign({}, src);
}
function cleanObject(obj) {
    if (Array.isArray(obj)) return obj;
    var tmp = clone(obj);
    if (typeof tmp.__ob__ !== "undefined") delete tmp.__ob__;
    return tmp;
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
    if ($(".selectpicker").length !== 0) {
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
    $("input").trigger("change");
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
    },
    confirm: function (params) {
        swal({
            title: params.title,
            text: params.text,
            type: 'warning',
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success',
            cancelButtonClass: 'btn btn-danger',
            confirmButtonText: 'Valider',
            buttonsStyling: false
        }).then(
            function (a) {
                params.valider();
            },
            function (dismiss) {
                //console.log("dismiss", dismiss);
            }
        );
    },
    modal: function (params) {
        swal({
            width: params.width,
            html: "<div id='modal' style='height:"+params.height+"; overflow-y:auto'></div>",
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success',
            cancelButtonClass: 'btn',
            confirmButtonText: 'Enregistrer',
            cancelButtonText: 'Fermer',
            buttonsStyling: false,
            showCloseButton:true,
            onOpen: (e) => {
                EV.getComponent("Router").LoadFor($("#modal"), params.url, params.load);
            }
        }).then(
            function (a) {
                params.valider();
            },
            function (dismiss) {
                //console.log("dismiss", dismiss);
            }
        );
    },
}

function sideBarFix() {
    if (isWindows) {
        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar('destroy');
        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar();
    }
}

var URL = {
    addParam: function (url, param) {
        return url + (url.indexOf("?") < 0 ? "?" : "&") + param;
    },
    addPart: function (url, part) {
       //console.log("addPart : " + url, part);
        return url + (url.slice(-1) === "/" ? "" : "/") + part;
    }
};

function GetId() {
    return '_' + Math.random().toString(36).substr(2, 9);
};

function cleanDBName(str) {
    return str
        .replace(/[éèëê]/g, "e")
        .replace(/[à]/g,"a")
        .replace(/[ç]/g,"c")
        .replace(/[^a-zA-Z0-9 ]/g, "")
        .replaceAll("  ", " ").replaceAll("  ", " ").replaceAll("  ", " ").replaceAll("  ", " ")
        .replaceAll(" ", "_");
}

function FormTypeToDbType(formType) {
    switch (formType) {
        case 'v-text':
            return "varchar(100)";
        case 'v-text-area':
            return "varchar(MAX)";
        case 'v-select':
            return "varchar(100)";
        case 'v-checkbox':
            return "int";
        case 'v-datepicker':
            return "DateTime";
        case 'v-email':
            return "varchar(100)";
        default:
            return "varchar(MAX)";
    }
}