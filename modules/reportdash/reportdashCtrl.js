var reportdash = angular.module('ReportDash', []);

reportdash.controller('reportdashCtrl', ['$scope', '$rootScope', 'YearlyReport', 'Regions',
  function($scope, $rootScope, YearlyReport, Regions) {


    $scope.years = ['2012-13', '2013-14', '2014-15'];

    Regions.fetch().then(function(data) {
      $scope.regions = data;
      $scope.$watch('selectedState', function() {
        fetchDistricts($scope.selectedState);
      });
    });









    YearlyReport.fetch().then(function(response) {
      $scope.yearlydata = response;
    });





    function fetchDistricts(selectedState) {
      $scope.districts = [];
      console.log(selectedState);
      $scope.districts = $scope.regions[1][selectedState];
    };


    // converting numbers to a generic digit
    		// leftPad(1, 2)  ---> 01
    		// leftPad(10, 3) ---> 010
    function leftPad(number, targetLength) {
      var output = number + '';
      while (output.length < targetLength) {
        output = '0' + output;
      }
      return output;
    }





  }
]);
