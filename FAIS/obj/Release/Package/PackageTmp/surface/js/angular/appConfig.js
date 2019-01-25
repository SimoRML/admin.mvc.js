var app_verison = 116;
var GLOBAL = {
	"config": {
		"devise" : "DH",
		"lang" : "fr",
		year : 2016
	},
	data: {},
	lang: LANGUES
};
var L;
var settings = {
	init: function(){
		// CLEAR ALL IF NW VERSION
		if(! localStorage.getItem("app_version")){
			console.log("NEW VERSION ", app_verison);
			localStorage.clear();
			localStorage.setItem("app_version", app_verison);
		}else{
			if(parseInt(localStorage.getItem("app_version")) < app_verison){
				console.log("NEW VERSION UP ", app_verison);
				localStorage.clear();
				localStorage.setItem("app_version", app_verison);
			} 
		}
		
		settings.getValues();
		L = GLOBAL.lang[GLOBAL.config.lang];
	},
	getValues: function(){
		for(var key in GLOBAL.config){
			if(! localStorage.getItem("setting_" + key)) 
				localStorage.setItem("setting_" + key, GLOBAL.config[key])
			GLOBAL.config[key] = localStorage.getItem("setting_" + key);
			
			//console.log("CONFIG[ "+key+" ]", GLOBAL.config[key]);
		}
		// DEFAULTS
		if(GLOBAL.config.year == 0 ) 
			GLOBAL.config.year = new Date().getFullYear();
	}
	,
	setValue : function(key, value){
		localStorage.setItem("setting_" + key, value);
	}
}; 
settings.init();

$(function(){
	$("#t_devise").val(GLOBAL.config.devise);
});