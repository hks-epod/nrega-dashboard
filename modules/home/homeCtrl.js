var home = angular.module('home', []);

home.controller('homeCtrl', ['$scope', '$rootScope',
  function($scope, $rootScope) {
    $scope.hello= 'hello';
  }
]);
