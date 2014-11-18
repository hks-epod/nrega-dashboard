var nregadash = angular.module('Nregadash', ['ngRoute', 'ReportDash', 'Pdp', 'VillageView', 'ui.select2', 'angular-loading-bar']);

nregadash.run(['uiSelect2Config', function(uiSelect2Config) {
  uiSelect2Config.allowClear = true;
}]);

nregadash.config(['$routeProvider', '$locationProvider',
  function($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(false);

    $routeProvider.
    when('/', {
      templateUrl: 'modules/reportdash/reportdashView.html',
      controller: 'reportdashCtrl'
    }).
    when('/pdp', {
      templateUrl: 'modules/pdp/pdpView.html',
      controller: 'pdpCtrl'
    }).
    when('/villageview', {
      templateUrl: 'modules/villageview/villageviewView.html',
      controller: 'villageviewCtrl'
    }).
    otherwise({
      redirectTo: '/'
    });
  }
]);
