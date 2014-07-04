reportdash.factory('YearlyReport', ['$http',
  function($http) {
    var YearlyReport = function(data) {
      angular.extend(this, data);
    };

    YearlyReport.fetch = function(code, year) {
      return $http.get('api/dashboard_report_yearly.aspx?'+params.code_type+'_code='  + params.code+'&' +'fin_year='+ year +'&type='+params.type)
        .then(function(response) {
          return response.data;
        });
    };

    return YearlyReport;
}]);

reportdash.factory('MonthlyReport', ['$http',
  function($http) {
    var YearlyReport = function(data) {
      angular.extend(this, data);
    };

    YearlyReport.fetch = function(code, year) {
      return $http.get('api/dashboard_report_monthly.aspx?'+params.code_type+'_code='  + params.code+'&' +'fin_year='+ year +'&type='+params.type)
        .then(function(response) {
          return response.data;
        });
    };

    return YearlyReport;
}]);


reportdash.factory('GPRegions', ['$http',
  function($http) {
    var GPRegions = function(data) {
      angular.extend(this, data);
    };

    GPRegions.fetch = function(blockcode, year) {
      return $http.get('api/dashboard_report_panchayat.aspx?block_code='+blockcode+'&fin_year='+ year)
        .then(function(response) {
          return response.data;
        });
    };

    return GPRegions;
}]);
