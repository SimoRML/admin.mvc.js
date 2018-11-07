function Router(id, target, options){
	BaseObject.call(this,id,"Router");
	this.target = target;
	this.options = options || {};
}
Router.prototype = Object.create(BaseObject.prototype);
Router.prototype.Init = function(){
	var me = this;
	if(typeof this.target !== "undefined") this.$element = $(this.target);	
	
	// Bind event : hash change
	$(window).on('hashchange', function() {
		var hash = window.location.hash;
		me.Load(hash);
	});
}
Router.prototype.LoadHomePage = function(){
	// Load first page if exists
	var hash = window.location.hash;
	this.Load(hash);	
}
Router.prototype.Load = function(page){
	if(page === ""){
		if(typeof this.options.home !== "undefined") page = this.options.home;
		else return;
	}
	var me = this;
    var url = page.replaceAll("#", "").replaceAll(".", "/"); // + "?v="+ (new Date().getTime());
	me.Trigger("show", "PagePreloader");
    me.$element.load(url, function (response, status, xhr) {
		me.Trigger("hide", "PagePreloader");
		if(status == "success"){
            updateDom();
			$("#menu a.selected").removeClass("selected");
            $("#menu a[href='" + page + "']").addClass("selected");
		}
	});
}