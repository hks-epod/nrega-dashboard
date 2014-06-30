var reportdash = angular.module('ReportDash', []);

reportdash.controller('reportdashCtrl', ['$scope', '$rootScope', 'YearlyReport', 'Regions',
  function($scope, $rootScope, YearlyReport, Regions) {

    $scope.years = ['2012-13', '2013-14', '2014-15'];

    Regions.fetch().then(function(data) {
      $scope.regions = data;
      $scope.$watch('selectedState', function() {
        fetchDistricts($scope.selectedState);
      });
      $scope.$watch('selectedDistrict', function() {
        fetchBlocks($scope.selectedState, $scope.selectedDistrict);
      });
    });

    function fetchDistricts(selectedState) {
      $scope.districts = [];
      $scope.districts = $scope.regions[1][selectedState];
    };

    function fetchBlocks(selectedState, selectedDistrict) {
      $scope.blocks = [];
      if (selectedState) $scope.blocks = $scope.regions[2][selectedState][selectedDistrict];
    };

    $scope.viewResults = function() {
      codetype = buildCode();
      console.log(codetype);
      YearlyReport.fetch().then(function(response) {
        $scope.yearlydata = response;
      });

    };

    function buildCode() {
      // Only State
      if ($scope.selectedState && !$scope.selectedDistrict && !$scope.selectedBlock) {
        return {
          code: leftPad($scope.selectedState),
          type: 'S'
        };
      };
      // State+District
      if ($scope.selectedState && $scope.selectedDistrict && !$scope.selectedBlock) {
        return {
          code: leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2),
          type: 'D'
        };
      };
      // State+District+Blocks
      if ($scope.selectedState && $scope.selectedDistrict && $scope.selectedBlock) {
        return {
          code: leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2) + leftPad($scope.selectedBlock, 3),
          type: 'B'
        };
      };
    };


    // converting numbers to a generic digit  : leftPad(1, 2)  ---> 01

    function leftPad(number, targetLength) {
      var output = number + '';
      while (output.length < targetLength) {
        output = '0' + output;
      }
      return output;
    };


  }
]);
