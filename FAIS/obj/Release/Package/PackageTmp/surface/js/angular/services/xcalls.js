app.factory('xcalls', ['$http', function($http) { 
  return $http.get('js/angular/appConfig.json') 
            .success(function(data) { 
              return data; 
            }) 
            .error(function(err) { 
              return err; 
            }); 
}]);