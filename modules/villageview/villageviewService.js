villageview.factory('Workers', ['$http',
  function($http) {
    var Workers = function(data) {
      angular.extend(this, data);
    };

    Workers.fetch = function() {
      return $http.get('api/data/workers.json')
        .then(function(response) {
          return response.data;
        });
    };

    return Workers;
  }
]);


villageview.factory('Works', ['$http',
  function($http) {
    var Works = function(data) {
      angular.extend(this, data);
    };

    Works.fetch = function() {
      return $http.get('api/data/works.json')
        .then(function(response) {
          return response.data;
        });
    };

    return Works;
  }
]);



villageview.factory('Musters', ['$http',
  function($http) {
    var Musters = function(data) {
      angular.extend(this, data);
    };

    Musters.fetch = function() {
      return $http.get('api/data/musters.json')
        .then(function(response) {
          return response.data;
        });
    };

    return Musters;
  }
]);

villageview.factory('Vstats', ['$http',
  function($http) {
    var Vstats = function(data) {
      angular.extend(this, data);
    };

    Vstats.fetch = function() {
      return $http.get('api/data/village_stats.json')
        .then(function(response) {
          return response.data;
        });
    };

    return Vstats;
  }
]);
