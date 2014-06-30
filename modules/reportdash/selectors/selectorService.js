reportdash.factory('Regions', ['$http',
  function($http) {

    var Regions = function(data) {
      angular.extend(this, data);
    };

    Regions.fetch = function(id) {
      return $http.get('api/geography.json')
        .then(function(response) {
          return response.data;
        });
    };

    return Regions;
}]);
