var home = angular.module('home', []);

home.controller('homeCtrl', ['$scope', '$rootScope','YearlyReport',
  function($scope, $rootScope, YearlyReport) {
    
    $scope.hello= 'hello';
    $scope.yearlydata=YearlyReport.testfetch;






  }
]);
