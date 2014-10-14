var pdp = angular.module('Pdp', []);

pdp.controller('pdpCtrl', ['$scope', '$window', '$location', 'Regions', 'GPRegions',
  function($scope, $window, $location, Regions, GPRegions) {


    $scope.years = ['2014-2015', '2013-2014', '2012-2013'];
    $scope.selectedYear = $scope.years[0];
    $scope.$watch('selectedYear', function() {

      Regions.fetch($scope.selectedYear).then(function(data) {
        $scope.regions = data;

      });
    });



  }
]);
