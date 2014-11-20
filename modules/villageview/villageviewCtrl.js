var villageview = angular.module('VillageView', []);

villageview.controller('villageviewCtrl', ['$scope', '$window', '$location', '$rootScope', 'Regions', 'GPRegions', 'Workers', 'Works', 'Musters',
  function($scope, $window, $location, $rootScope, Regions, GPRegions, Workers, Works, Musters) {
    $scope.isStat = false;
    $scope.switchview = function() {
      $scope.isStat = !$scope.isStat;
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

      Works.fetch().then(function(response) {
        $scope.works = response;
      });

      Musters.fetch().then(function(response) {
        $scope.musters = response;
      });

      Workers.fetch().then(function(response) {
        $scope.workers = response;
      });



    };


    function loadMustersbyWork(workcode) {
      $scope.filteredMusters = [];
      $scope.musters.forEach(function(muster, index) {
        if (muster.work_code == workcode) {
          $scope.filteredMusters.push(muster);
        }
      });
    }

    function loadWorkersbyMuster(muster) {
      $scope.workers = muster.workers;
    }



    $scope.loadMusters = function(work) {
      $scope.activeWork = work;
      loadMustersbyWork(work.work_code);
    };

    $scope.loadWorkers = function(muster) {
      $scope.activeMuster = muster;
      loadWorkersbyMuster(muster);
    };



  }
]);
