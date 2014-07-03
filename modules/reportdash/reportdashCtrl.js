var reportdash = angular.module('ReportDash', []);

reportdash.controller('reportdashCtrl', ['$scope', '$rootScope', 'YearlyReport', 'Regions',
  function($scope, $rootScope, YearlyReport, Regions) {

    ///////////////////////
    //  Region Handelers //
    ///////////////////////
    
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

    function buildCode() {
      // Only State
      if ($scope.selectedState && !$scope.selectedDistrict && !$scope.selectedBlock) {
        return {
          code: leftPad($scope.selectedState),
          type: 'S'
        };
      };
      // State + District
      if ($scope.selectedState && $scope.selectedDistrict && !$scope.selectedBlock) {
        return {
          code: leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2),
          type: 'D'
        };
      };
      // State + District + Blocks
      if ($scope.selectedState && $scope.selectedDistrict && $scope.selectedBlock) {
        return {
          code: leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2) + leftPad($scope.selectedBlock, 3),
          type: 'B'
        };
      };
    };

    // Generic Number Convertor Function: leftPad(1, 2) ---> 01

    function leftPad(number, targetLength) {
      var output = number + '';
      while (output.length < targetLength) {
        output = '0' + output;
      }
      return output;
    };

    //////////////////////////
    // Vizulization Loading //
    //////////////////////////
    $scope.vizconfig = {
      bindto: '#chart',
      data: {
        x: 'x',
        columns: [
                  ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
                  ['Demand Registered', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],
                  ['Labour Budget', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],
              ],
      },
      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%m'
          }
        }
      }
    };

    $scope.vizconfig2 = {
      bindto: '#chart2',
      data: {
        x: 'x',
        columns: [
                  ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
                  ['Demand Registered', 30, 20, 10, 40, 150, 250, 30, 200, 100, 400, 150, 250],
                  ['Labour Budget', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],
              ],
      },
      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%m'
          }
        }
      }
    };


    //////////////////////////
    //      View Results    //
    //////////////////////////

    $scope.viewResults = function() {
      codetype = buildCode();
      // YearlyReport.fetch().then(function(response) {
      //   $scope.yearlydata = response;
      // });
      $scope.yearlydata = YearlyReport.testfetch;
    };



  }
]);
