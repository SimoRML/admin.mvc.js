function BaseObject(id, type){
	this.Id = typeof id === "undefined" ? type : id;
	this.Type = type;
	this.Childs = {};
	this.Parent = null;
	this.$element = $("#" + this.Id).length > 0 ? $("#" + this.Id) : null;
	this.EV;
}
/// Functions
BaseObject.prototype.Init = function () {
    // if(this.$element == null) this.$element = $(document);
};
BaseObject.prototype.getComponent = function (componentName) {
    return this.Childs[componentName];
};
BaseObject.prototype.CreateObject = function(o){
	o.Parent = this;	
	//o.Id = this.Id + '.' + o.Id; 
	this.Childs[o.Id] = o;
	
	if(this instanceof Environement) o.EV = this;
	else o.EV = o.Parent.EV;
	
	o.Trigger("Init");
};
BaseObject.prototype.Trigger = function(fct, targetId){
	if(typeof targetId === "undefined")
		this.EV.Action(this.Id, fct);
	else 
		this.EV.Action(targetId, fct);
};
BaseObject.prototype.TriggerFor = function(targetId, fct){
	this.Trigger(fct, targetId);
};
BaseObject.prototype.dump = function(){
	console.log(this.Id + " :: " + this.Json());
};
BaseObject.prototype.GetTemplate = function(){
	if(this.$element.length <= 0) return "";
	return this.$element.html();
};
