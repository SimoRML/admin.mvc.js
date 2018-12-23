function Preloader(id){
	BaseObject.call(this,id,"Preloader");
}
Preloader.prototype = Object.create(BaseObject.prototype);
Preloader.prototype.Init = function(){
	
}
Preloader.prototype.hide = function(){
	this.$element.hide();
}

Preloader.prototype.show = function(){
	this.$element.show();
}