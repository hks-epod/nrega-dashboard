var villageview = angular.module('VillageView', []);

villageview.controller('villageviewCtrl', ['$scope', '$window', '$location', '$modal', '$rootScope', 'Regions', 'GPRegions', 'Workers', 'Works', 'Musters', 'Vstats',
    function($scope, $window, $location, $modal, $rootScope, Regions, GPRegions, Workers, Works, Musters, Vstats) {

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
        };

        $scope.parseDate = function(jsonDate) {
            if (jsonDate == '')
                newdate = '';
            else

                newdate = new Date(jsonDate.replace(/(\d{2})\/(\d{2})\/(\d{4})/, "$2/$1/$3"));
            return newdate;
        }


        // Village View    --- Initialization
        $scope.isStat = false;
        $scope.switchview = function() {
            $scope.isStat = !$scope.isStat;
        };
        Vstats.fetch().then(function(response) {
            $scope.vstats = response;
        });

        var columnList = ['WORKS', 'WORKERS', 'MUSTERS'];
        $scope.column = {
            1: {
                'status': false
            },
            2: {
                'status': false
            },
            3: {
                'status': false
            }
        };
        $scope.activeRow = {
            work: {},
            worker: {},
            muster: {}
        };

        // Functions
        function fetchColumnResults(colName, params) {
            if (colName == 'WORKS') {
                Works.fetch().then(function(response) {
                    $scope.works = response;
                });
            };
            if (colName == 'MUSTERS') {
                Musters.fetch().then(function(response) {
                    $scope.musters = response;
                });
            };
            if (colName == 'WORKERS') {
                Workers.fetch().then(function(response) {
                    $scope.workers = response;
                });
            };
        }

        function whichColumn(colName) {
            columnList.forEach(function(value, index) {
                if (value == colName) return index
            });

        }

        // Load Column 
        $scope.loadColumn = function(colName, colNo) {
            $scope.column[colNo].name = colName;
            $scope.column[colNo].status = true;

            if (colNo == 2) {
                columnList.forEach(function(value) {
                    if (value != $scope.column['1'].name && value != $scope.column['2'].name) {
                        $scope.column[3].name = value;
                        $scope.column[3].status = true;
                    }
                });
            }

            if (colNo == 1) fetchColumnResults(colName);

        };

        // Load Row
        $scope.loadRow = function(parentColName, entity) {

            parentColNo = whichColumn(parentColName);

            if (parentColNo == 3) {
                return 0;
            } else if {
                childColName = $scope.column[parentColNo + 1].name;
            }

            $scope.activeRow[parentColName] = entity;

            //Prepare params here or in fetchColumnResults

            fetchColumnResults(childColName, param);

        }

        // Unload Column
        $scope.unloadCol = function(colNo) {
            if (colNo === 1) {
                $scope.column = {
                    1: {
                        'status': false,
                        'name': null
                    },
                    2: {
                        'status': false,
                        'name': null
                    },
                    3: {
                        'status': false,
                        'name': null
                    }
                };
                $scope.activeRow = {
                    work: {},
                    worker: {},
                    muster: {}
                };
            } else if (colNo === 2) {
                $scope.activeRow[$scope.column['2'].name]= null;
                $scope.column = {
                    2: {
                        'status': false,
                        'name': null
                    },
                    3: {
                        'status': false,
                        'name': null
                    }
                };
                $scope.activeRow[]

            } else if (colNo === 3) {
                $scope.column = {
                    3: {
                        'status': false,
                        'name': null
                    }
                };
            }

        };








        function extend(a, b) {
            for (var key in b)
                if (b.hasOwnProperty(key))
                    a[key] = b[key];
            return a;
        }
        $scope.col2 = false;
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







        $scope.open = function(item) {
            var modalInstance = $modal.open({
                templateUrl: 'myModalContent.html',
                controller: 'ModalInstanceCtrl',
                size: 'lg',
                resolve: {
                    detailItem: function() {
                        return item;
                    }
                }
            });
        };





    }
]);



villageview.controller('ModalInstanceCtrl', function($scope, $modalInstance, detailItem) {
    $scope.detailItem = detailItem;
    $scope.cancel = function() {
        $modalInstance.dismiss('cancel');
    };
});
