var villageview = angular.module('VillageView', []);

villageview.controller('villageviewCtrl', ['$scope', '$window', '$location', '$modal', '$rootScope', 'Regions', 'GPRegions', 'Workers', 'Works', 'Musters',
  function($scope, $window, $location, $modal, $rootScope, Regions, GPRegions, Workers, Works, Musters) {
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

    $scope.selectedState = '4';
    $scope.selectedDistrict = '10';
    $scope.selectedBlock = '10';

    $scope.viewResults = function() {};
    Works.fetch().then(function(response) {
      $scope.works = response;
    });
    Musters.fetch().then(function(response) {
      $scope.musters = response;
    });
    Workers.fetch().then(function(response) {
      $scope.workers = response;
    });



    // Load Column 
    $scope.col1 = false;
    $scope.col2 = false;
    $scope.loadColumn = function(colName) {
      $scope.col1 = true;
    };
    $scope.loadColumn2 = function(colName) {
      $scope.col2 = true;
    };


    function loadMustersbyWork(workcode) {
      $scope.filteredMusters = [];
      $scope.musters.forEach(function(muster, index) {
        if (muster.work_code == workcode) {
          $scope.filteredMusters.push(muster);
        }
      });
    };


    function extend(a, b) {
      for (var key in b)
        if (b.hasOwnProperty(key))
          a[key] = b[key];
      return a;
    }

    function loadWorkersbyMuster(muster) {
      $scope.filteredWorkers = [];

      muster.workers.forEach(function(filterWorker) {
        $scope.workers.forEach(function(worker) {
          if (filterWorker.worker_code == worker.worker_code) {
            $scope.filteredWorkers.push(extend(filterWorker, worker));
          }
        });
      });


    }



    $scope.loadMusters = function(work) {
      $scope.activeWork = work;
      $scope.filteredWorkers = [];
      $scope.activeMuster = {};
      loadMustersbyWork(work.work_code);
    };

    $scope.loadWorkers = function(muster) {
      $scope.activeMuster = muster;
      loadWorkersbyMuster(muster);
    };

    $scope.items = ['item1', 'item2', 'item3'];

    $scope.open = function(size) {

      var modalInstance = $modal.open({
        templateUrl: 'myModalContent.html',
        controller: 'ModalInstanceCtrl',
        size: size,
        resolve: {
          items: function() {
            return $scope.items;
          }
        }
      });

      modalInstance.result.then(function(selectedItem) {
        $scope.selected = selectedItem;
      }, function() {
        // $log.info('Modal dismissed at: ' + new Date());
      });
    };





  }
]);



villageview.controller('ModalInstanceCtrl', function($scope, $modalInstance, items) {

  $scope.items = items;
  $scope.selected = {
    item: $scope.items[0]
  };

  $scope.ok = function() {
    $modalInstance.close($scope.selected.item);
  };

  $scope.cancel = function() {
    $modalInstance.dismiss('cancel');
  };
});
