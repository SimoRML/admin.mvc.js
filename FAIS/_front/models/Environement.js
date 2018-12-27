var EV;
// ObjectStack = {};

function Environement(id){
	BaseObject.call(this,id,"Environement");
	this.$element = $("body");
	this.TL /* Time Line :: array of object*/ = {};
	this.FPS = 500;
	this.Events = {};
	this.TimeLineInterval = 1;
	this.T = -1 *  this.TimeLineInterval;
	EV = this;
	this.timer = setInterval(function(){
		EV.T ++;
		// console.log(EV.T);
		// Handle Update
		for(var i in EV.Childs){
			if(typeof EV.Childs[i].Update !== "undefined") EV.Childs[i].Update();
		}
		
		//Handle triggered actions
		if(typeof EV.TL["T" + EV.T] !== "undefined"){
			var actions = EV.TL["T" + EV.T].actions;
			delete EV.TL["T" + EV.T];
			for(var i = 0; i < actions.length; i++){
                //console.log("EV.Childs['"+actions[i].id+"']."+actions[i].fct+"()");
				eval("EV.Childs[actions[i].id]."+actions[i].fct+"()");
			}
		}
		
	}, this.FPS);
}
Environement.prototype = Object.create(BaseObject.prototype);
Environement.prototype.Action = function(id, fct){
	if(typeof this.TL["T" + (this.T + this.TimeLineInterval)] === "undefined"){
		this.TL["T" + (this.T + this.TimeLineInterval)] = { actions: [
			{
				id : id,
				fct : fct
			}
		] }
		return;
	};
	this.TL["T" + (this.T + this.TimeLineInterval)].actions.push({
		id : id,
		fct : fct
	});	
};
Environement.prototype.Event = function(e){
	if(typeof this.Events["T" + (this.T + this.TimeLineInterval)] === "undefined"){
		this.Events["T" + (this.T + this.TimeLineInterval)] = { actions: [
			{
				//id : id,
				event : e
			}
		] }
		return;
	};
	this.Events["T" + (this.T + this.TimeLineInterval)].actions.push({
		event : e
	});	
};