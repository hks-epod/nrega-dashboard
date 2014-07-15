var nregadash = angular.module('Nregadash', ['ngRoute', 'ReportDash','ui.select2','angular-loading-bar']);

nregadash.run(['uiSelect2Config', function(uiSelect2Config) {
    uiSelect2Config.allowClear= true;
}]);

nregadash.config(['$routeProvider', '$locationProvider',
  function($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(false);

    $routeProvider.
    when('/', {
      templateUrl: 'modules/reportdash/reportdashView.html',
      controller: 'reportdashCtrl'
    }).
    otherwise({
      redirectTo: '/'
    });
  }]);
