//require("models/BaseObject");
/*---------------- DIV ----------------------*/
function Div(id){
	BaseObject.call(this,id,"Div");
}
Div.prototype = Object.create(BaseObject.prototype);
Div.prototype.Init = function(){
	var html = this.GetTemplate();
	this.$element = $(html);
	this.Parent.$element.html(this.$element);
}
Div.prototype.Render = function(){
	// this.$element.find("span", 
}

Div.prototype.Update = function(){
	this.$element.find(".TL").html(Object.keys(EV.TL).join(" - "));
	this.$element.find(".EV").html(Object.keys(EV.Events).join(" - "));
}

Div.prototype.FixedUpdate = function(){
	this.$element.find(".TL").html(Object.keys(EV.TL).join(" - "));
	this.$element.find(".EV").html(Object.keys(EV.Events).join(" - "));
}