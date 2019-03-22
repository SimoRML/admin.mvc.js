var baseUrl = "/";
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
        this.show(text, 'danger', "error");
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
            html: "<div id='modal' style='height:" + params.height + "; overflow-y:auto; text-align:left'></div>",
            showCancelButton: true,
            confirmButtonClass: 'btn btn-success',
            cancelButtonClass: 'btn',
            confirmButtonText: 'Enregistrer',
            cancelButtonText: 'Fermer',
            buttonsStyling: false,
            showCloseButton: true,
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
    }
};

function sideBarFix() {
    if (isWindows) {
        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar('destroy');
        $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar();
        // $("#minimizeSidebar").trigger("click");
        // $('.sidebar .sidebar-wrapper, .main-panel').perfectScrollbar('destroy');


    }
}

window.URL.addParam = function (url, param) {
    return url + (url.indexOf("?") < 0 ? "?" : "&") + param;
};
window.URL.addPart = function (url, part) {
        // console.log("addPart : ", url, " part : ", part);
        if (typeof part.slice !== "undefined" && part[0] === "/") part = part.substr(1);
        url = url + (url.slice(-1) === "/" ? "" : "/") + part;
        //console.log("addPart -> " + url);
        return url;
};
window.URL.queryString = function (param) {
    var vars = window.location.href.split("?")[1];
    if (typeof vars === "undefined") return null;
    vars = vars.split("&");
    var query_string = {};
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        var key = decodeURIComponent(pair[0]);
        var value = decodeURIComponent(pair[1]);
        // If first entry with this name
        if (typeof query_string[key] === "undefined") {
            query_string[key] = decodeURIComponent(value);
            // If second entry with this name
        } else if (typeof query_string[key] === "string") {
            var arr = [query_string[key], decodeURIComponent(value)];
            query_string[key] = arr;
            // If third or later entry with this name
        } else {
            query_string[key].push(decodeURIComponent(value));
        }
    }
    if (typeof query_string[param] === "undefined") return null;
    return query_string[param];
};
/*
var URL = {
    addParam: function (url, param) {
        return url + (url.indexOf("?") < 0 ? "?" : "&") + param;
    },
    addPart: function (url, part) {
        //console.log("addPart : " + url, part);
        if (typeof part.slice !== "undefined" && part[0] === "/") part = part.substr(1);
        url = url + (url.slice(-1) === "/" ? "" : "/") + part;
        //console.log("addPart -> " + url);
        return url;
    }
};
*/

function GetId() {
    return '_' + Math.random().toString(36).substr(2, 9);
}

function cleanDBName(str) {
    return str
        .replace(/[éèëê]/g, "e")
        .replace(/[à]/g, "a")
        .replace(/[ç]/g, "c")
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
            return "nvarchar(MAX)";
    }
}


function getBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = () => resolve(reader.result);
        reader.onerror = error => reject(error);
    });
}


function b64toBlob(b64Data, contentType, sliceSize) {
  contentType = contentType || '';
  sliceSize = sliceSize || 512;

  var byteCharacters = atob(b64Data);
  var byteArrays = [];

  for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
    var slice = byteCharacters.slice(offset, offset + sliceSize);

    var byteNumbers = new Array(slice.length);
    for (var i = 0; i < slice.length; i++) {
      byteNumbers[i] = slice.charCodeAt(i);
    }

    var byteArray = new Uint8Array(byteNumbers);

    byteArrays.push(byteArray);
  }

  var blob = new Blob(byteArrays, {type: contentType});
  return blob;
}

function resizeImg(img, width, height) {

    // create an off-screen canvas
    var canvas = document.createElement('canvas'),
        ctx = canvas.getContext('2d');

    // set its dimension to target size
    canvas.width = width;
    canvas.height = height;

    // draw source image into the off-screen canvas:
    ctx.drawImage(img, 0, 0, width, height);

    // encode image to data-uri with base64 version of compressed image
    return canvas.toDataURL();
}