var nregadash = angular.module('Nregadash', ['ngRoute', 'home']);

nregadash.config(['$routeProvider', '$locationProvider',
  function($routeProvider, $locationProvider) {
    $locationProvider.html5Mode(false);

    $routeProvider.
    when('/', {
      templateUrl: 'modules/home/home.html',
      controller: 'homeCtrl'
    }).
    otherwise({
      redirectTo: '/'
    });
  }]);
