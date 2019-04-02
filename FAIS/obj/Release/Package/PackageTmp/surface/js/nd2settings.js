$(function() {

  // Initialize nativeDroid2

  $.nd2({
    stats : {
      analyticsUA: false // Your UA-Code for Example: 'UA-123456-78'
    },
    advertising : {
      active : false, // true | false
      path : "/examples/fragments/adsense/", // Define where the Ad-Templates are: For example: 
      extension : ".html" // Define the Ad-Template content Extension (Most case: ".html" or ".php")
    }
  });


});
