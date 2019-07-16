String.prototype.trim=function(){return this.replace(/^\s+|\s+$/g, '');};
String.prototype.ltrim=function(){return this.replace(/^\s+/,'');};
String.prototype.rtrim=function(){return this.replace(/\s+$/,'');};
String.prototype.fulltrim=function(){return this.replace(/(?:(?:^|\n)\s+|\s+(?:$|\n))/g,'').replace(/\s+/g,' ');};
if ( !String.prototype.contains ) {
    String.prototype.contains = function() {
        return String.prototype.indexOf.apply( this, arguments ) !== -1;
    };
}

var currentTab = 1;
var maxTabs=0;
var client;
$( document ).on( "ready", function(){
	/*
	tabCnt = $("#ulTabs li").length;
  $("body").on("swipeleft",function(){
    if(currentTab < tabCnt){
		currentTab ++;
		$("#tab_" + currentTab).trigger("click");
	}
  });   
  $("body").on("swiperight",function(){
    if(currentTab > 1){
		currentTab --;
		$("#tab_" + currentTab).trigger("click");
	}
  });  
  */
	//COULAGE
	$( "#tab_1" ).click(function() {
		  
	});
	//SETTINGS
	$( "#tab_2" ).click(function() {
		  	   
	});
  
  document.addEventListener("deviceready", onDeviceReady, false);  
  $("input").keydown(ensureNumeric);
});
function ensureNumeric(e){
	/*
	// Allow: backspace, delete, tab, escape, enter and .
	if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190, 109, 188, 189]) !== -1 ||
		 // Allow: Ctrl+A, Command+A
		(e.keyCode == 65 && ( e.ctrlKey === true || e.metaKey === true ) ) || 
		 // Allow: home, end, left, right, down, up
		(e.keyCode >= 35 && e.keyCode <= 40)) {
			 // let it happen, don't do anything
			 return;
	}
	// Ensure that it is a number and stop the keypress
	if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
		e.preventDefault();
	}
	*/
}
function onDeviceReady(){
	console.log("DEVICE IS READY");
}
