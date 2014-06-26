var home = angular.module('home', []);

home.controller('homeCtrl', ['$scope', '$rootScope','YearlyReport',
  function($scope, $rootScope, YearlyReport) {
    
    YearlyReport.fetch().then(function(response){
      $scope.yearlydata= response;
    });






  }
]);
