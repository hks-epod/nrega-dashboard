reportdash.factory('Regions', ['$http',
  function($http) {

    var Regions = function(data) {
      angular.extend(this, data);
    };

    Regions.fetch = function(year) {
      return $http.get('api/sdb'+ year +'.txt')
        .then(function(response) {
          return response.data;
        });
    };

    return Regions;
}]);
