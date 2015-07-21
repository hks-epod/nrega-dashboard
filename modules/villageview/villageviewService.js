villageview.factory('Workers', ['$http',
  function($http) {
    var Workers = function(data) {
      angular.extend(this, data);
  };

  Workers.fetchDetail = function (params) {
      return $http({
          method: 'POST',
          url: '../nregapost/api_api3.asmx/API_API3_Worker_Detail',
          data: params,
          headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
      })
        .then(function (response) {
            return JSON.parse(response.data.reponse_data);
        });
  };

    Workers.fetch = function(params) {
      return  $http({
        method: 'POST',
        url: '../nregapost/API_API3.asmx/API_API3_Workers',
         data: params,
        headers: {'Content-Type': 'application/x-www-form-urlencoded'}
      })
        .then(function(response) {
            return JSON.parse(response.data.reponse_data) ;
        });
    };

    return Workers;
  }
]);

  villageview.factory('Works', ['$http',
  function ($http) {
      var Works = function (data) {
          angular.extend(this, data);
      };

      Works.fetchDetail = function (params) {
          return $http({
              method: 'POST',
              url: '../nregapost/api_api1.asmx/API_API1_GetWorkDetails',
              data: params,
              headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
          })
        .then(function (response) {
            return JSON.parse(response.data.reponse_data); ;
        });
      };
      Works.fetch = function (params) {
          console.log(params);
          return $http({
              method: 'POST',
              url: '../nregapost/api_api1.asmx/API_API1_GetWorks',
              data: params,
              headers: { 'Content-Type': 'application/x-www-form-urlencoded' }
          })
        .then(function (response) {
            return JSON.parse(response.data.reponse_data);
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

    Musters.fetch = function(params) {
      return  $http({
        method: 'POST',
        url: '../nregapost/API_API2.asmx/API_API2',
         data: params,
        headers: {'Content-Type': 'application/x-www-form-urlencoded'}
      })
        .then(function(response) {
            return JSON.parse(response.data.reponse_data);
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

    Vstats.fetch = function(params) {
      return  $http({
        method: 'POST',
        url: '../nregapost/API_API4.asmx/API_API4',
        data: "panchayat_code="+params,
        headers: {'Content-Type': 'application/x-www-form-urlencoded'}
      })
        .then(function(response) {
          return response.data;
        });
    };

    return Vstats;
  }
]);
