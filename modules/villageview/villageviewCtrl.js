var villageview = angular.module('VillageView', []);

villageview.controller('villageviewCtrl', ['$scope', '$window', '$location', '$rootScope', 'YearlyReport', 'Regions', 'GPRegions', 'MonthlyReport', 'YearlyReportNational', 'MonthlyReportNational',
  function($scope, $window, $location, $rootScope, YearlyReport, Regions, GPRegions, MonthlyReport, YearlyReportNational, MonthlyReportNational) {
    $scope.isTable = false;
    $scope.switchview = function() {
      $scope.isTable = !$scope.isTable;
    };

    //*****************[ Region Handelers ]*******************//
    $scope.years = ['2014-2015', '2013-2014', '2012-2013'];
    $scope.selectedYear = $scope.years[0];
    Regions.fetch($scope.selectedYear).then(function(data) {
      $scope.regions = data;
    });


    $scope.$watch('selectedState', function() {
      if ($scope.regions) fetchDistricts($scope.selectedState)
    });
    $scope.$watch('selectedDistrict', function() {
      if ($scope.regions) fetchBlocks($scope.selectedState, $scope.selectedDistrict);
    });
    $scope.$watch('selectedBlock', function() {
      if ($scope.regions) fetchGPs(leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2) + leftPad($scope.selectedBlock, 3));
    });

    function fetchDistricts(selectedState) {

      $scope.districts = [];
      $scope.selectedDistrict = null;
      $scope.districts = $scope.regions[1][selectedState];
    };

    function fetchBlocks(selectedState, selectedDistrict) {
      $scope.blocks = [];
      $scope.selectedBlock = null;
      if (selectedState) $scope.blocks = $scope.regions[2][selectedState][selectedDistrict];
    };

    function fetchGPs(selectedBlock) {
      $scope.gps = [];
      $scope.selectedGP = null;
      if (selectedBlock && $scope.selectedYear) {
        GPRegions.fetch(selectedBlock, $scope.selectedYear).then(function(response) {
          $scope.gps = response[0];
        });
      };
    };

    function buildCode() {
      // Only State
      if ($scope.selectedState && !$scope.selectedDistrict && !$scope.selectedBlock && !$scope.selectedGP) {
        return {
          code_type: 'state',
          code: leftPad($scope.selectedState, 2),
          type: 's'
        };
      };
      // State + District
      if ($scope.selectedState && $scope.selectedDistrict && !$scope.selectedBlock && !$scope.selectedGP) {
        return {
          code_type: 'district',
          code: leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2),
          type: 'd'
        };
      };
      // State + District + Blocks
      if ($scope.selectedState && $scope.selectedDistrict && $scope.selectedBlock && !$scope.selectedGP) {
        return {
          code_type: 'block',
          code: leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2) + leftPad($scope.selectedBlock, 3),
          type: 'b'
        };
      };
      // State + District + Blocks + GP
      if ($scope.selectedState && $scope.selectedDistrict && $scope.selectedBlock && $scope.selectedGP) {
        return {
          code_type: 'panchayat',
          code: leftPad($scope.selectedGP, 10),
          type: 'p'
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

    $scope.getKeyByValue = function(obje, value) {
      for (var prop in obje) {
        if (obje.hasOwnProperty(prop)) {
          if (obje[prop] == value) {
            return prop;
          }
        }
      }
    }

    //////////////////////////
    //      View Results    //
    //////////////////////////

    $scope.viewResults = function() {


      if ($scope.selectedYear && !$scope.selectedState && !$scope.selectedDistrict && !$scope.selectedBlock && !$scope.selectedGP) {
        // $window.ga('send', 'event', $scope.selectedYear, 'National');

        // 


      } else {
        params = buildCode();
        // $window.ga('send', 'event', $scope.selectedYear, params.code_type, params.code);


      }
    };




  }
]);
