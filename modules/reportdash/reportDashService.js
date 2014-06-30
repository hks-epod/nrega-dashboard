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


    YearlyReport.fetch = function(id) {
      return $http.get('api/dashboard_report_monthly.aspx?state_code=29&fin_year=2014-2015&type=s' + id)
        .then(function(response) {
          return response.data;
        });
    };

    return YearlyReport;
}]);
