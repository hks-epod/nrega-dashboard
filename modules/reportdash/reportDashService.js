reportdash.factory('YearlyReport', ['$http',
  function($http) {
    var YearlyReport = function(data) {
      angular.extend(this, data);
    };

    YearlyReport.testfetch = [{
      "state_code": "29",
      "demand_register": 8677290
    }, {
      "state_code": "29",
      "labour_budget": 383100310
    }, {
      "state_code": "29",
      "work_alloted": 4324800
    }, {
      "state_code": "29",
      "payable_amount": 107137263.5,
      "payable_days": 1884517
    }, {
      "state_code": "29",
      "approved_days": 428,
      "rejected_days": 411
    }];


    YearlyReport.fetch = function(code, year) {
      return $http.get('api/dashboard_report_yearly.aspx?'+params.code_type+'='  + params.code+'&' +'fin_year='+ year +'&type='+params.type)
        .then(function(response) {
          return response.data;
        });
    };

    return YearlyReport;
}]);
