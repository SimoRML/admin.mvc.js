function Router(id, target, options){
	BaseObject.call(this,id,"Router");
	this.target = target;
    this.options = options || {};
    this.vueApp = null;
}
Router.prototype = Object.create(BaseObject.prototype);
Router.prototype.Init = function () {
    var me = this;
    if (typeof this.target !== "undefined") this.$element = $(this.target);

    // Bind event : hash change
    $(window).on('hashchange', function () {
        var hash = window.location.hash;
        me.Load(hash);
    });
};
Router.prototype.LoadHomePage = function () {
    // Load first page if exists
    var hash = window.location.hash;
    this.Load(hash);
};
Router.prototype.Load = function (page) {
    if (page === "") {
        if (typeof this.options.home !== "undefined") page = this.options.home;
        else return;
    }
    var me = this;
    me.vueApp = null;
    var url = page.replaceAll("#", "").replaceAll(".", "/"); // + "?v="+ (new Date().getTime());
    me.Trigger("show", "PagePreloader");
    me.$element.load(URL.addPart(baseUrl,url), function (response, status, xhr) {
        me.Trigger("hide", "PagePreloader");

        // log.red("ROUTER LOAD ", status, xhr);
        if (status === "success") {
            /*
            if ($(".vue-app").length > 0 && me.vueApp === null) {
                me.vueApp = new Vue(
                    {
                        el: '.vue-app',
                        store
                    });
            }
            */
            updateDom();
            $("#menu a.selected").removeClass("selected");
            $("#menu a[href='" + page + "']").addClass("selected");
        } else if (status == "error") {
            if(xhr.status == 401) NOTIF.error("Accès refusé !");
        }
    });
};
Router.prototype.LoadFor = function ($target, url, load) {
    var me = this;
    $target.addClass("preLoader");
    $target.load(url, function (response, status, xhr) {
        $target.removeClass("preLoader");
        if (status === "success") {
            updateDom();
            if (typeof load === "function") load();
        }
    });
};