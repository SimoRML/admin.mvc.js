/*---------------- DIV ----------------------*/
function DataSource(id, config){
	BaseObject.call(this,id,"DataSource");
    this.config = config || [];
    this.config.loginPath = "/home/login";
    this.logout = function () {
        localStorage.removeItem("_tkn_");
        window.location = this.config.loginPath + "?returnUrl=";
    };
    this.getToken = function () {
        if (localStorage.getItem("_tkn_") === null) {
            this.logout();
        } else {
            this.config.token = localStorage.getItem("_tkn_");
        }
    };
    if (window.location.pathname !== this.config.loginPath) this.getToken();
}
DataSource.prototype = Object.create(BaseObject.prototype);
DataSource.prototype.Init = function () {
    var me = this;
    for (var i in this.config.sources) {
        var source = this.config.sources[i];

        /* EVENT */
        if (typeof source["on"] !== "undefined") {
            $(source.on.el).on(source.on.event, function () {
                if (typeof source.on.before === "function") {
                    if (source.on.before(source))
                        me.ExecuteSource(source);
                } else {
                    me.ExecuteSource(source);
                }
            });
        } else this.ExecuteSource(source);
        /* END EVENT */
    }
};

DataSource.prototype.ExecuteSource = function (source) {
    var me = this;
    var settings = {
        "async": true,
        "crossDomain": false,
        "url": this.config.baseUrl + source.url,
        "method": typeof source.method === "undefined" ? "GET" : source.method,
        "headers": {
            "Cache-Control": "no-cache",
            "content-type": typeof source["content-type"] === "undefined" ? "application/json" : source["content-type"],
            "authorization": "Bearer " + me.config.token
        }
    };
    if (typeof source["data"] !== "undefined") {
        settings["data"] = source["data"];
    }

    // console.log("ExecuteSource", settings);
    $.ajax(settings)
        .done(function (response) {
            try { response = JSON.parse(response); } catch{ }
                
            var html = "";

            // REPEAT
            if (typeof source.repeat === "function")
                for (var key in response) html += source.template(key, response[key]);

            // TARGET
            if (typeof source.target === "function")
                $(source.target).html($(html));

            // LOAD COMPLETE
            if (typeof source.loadComplete === "function")
                source.loadComplete(me, response);
        })
        .fail(function (response) {
            // console.log("fail", response);
            if (response.status === 401) {
                me.logout();
            }
            // LOAD COMPLETE
            if (typeof source.fail === "function")
                source.fail(response);
        });
}
