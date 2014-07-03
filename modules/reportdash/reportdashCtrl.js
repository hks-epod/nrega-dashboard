var reportdash = angular.module('ReportDash', []);

reportdash.controller('reportdashCtrl', ['$scope', '$rootScope', 'YearlyReport', 'Regions', 'GPRegions',
  function($scope, $rootScope, YearlyReport, Regions, GPRegions) {
    $scope.isTable = false;
    $scope.switchview = function() {
      $scope.isTable = !$scope.isTable;
    };


    ///////////////////////
    //  Region Handelers //
    ///////////////////////

    $scope.years = ['2012-2013', '2013-2014', '2014-2015'];
    Regions.fetch().then(function(data) {
      $scope.regions = data;
      $scope.$watch('selectedState', function() {
        fetchDistricts($scope.selectedState);
      });
      $scope.$watch('selectedDistrict', function() {
        fetchBlocks($scope.selectedState, $scope.selectedDistrict);
      });
      $scope.$watch('selectedBlock', function() {
        fetchGPs($scope.selectedBlock);
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

    function fetchGPs(selectedBlock) {
      $scope.gps = [];
      if (selectedBlock) {
        GPRegions.fetch($scope.selectedBlock, $scope.selectedYear).then(function(response) {
          $scope.gps = response[0];
        });
      };

    };

    function buildCode() {
      // Only State
      if ($scope.selectedState && !$scope.selectedDistrict && !$scope.selectedBlock) {
        return {
          code_type: 'state',
          code: leftPad($scope.selectedState),
          type: 's'
        };
      };
      // State + District
      if ($scope.selectedState && $scope.selectedDistrict && !$scope.selectedBlock) {
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
          code: leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2) + leftPad($scope.selectedBlock, 3) + leftPad($scope.selectedGP, 10),
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

    //////////////////////////
    // Vizulization Loading //
    //////////////////////////
    $scope.demand_labourbudget_chart = {
      bindto: '#demand_labourbudget_chart',
      data: {
        x: 'x',
        columns: [
                  ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
                  ['Demand Registered', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],
                  ['Labour Budget', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],
              ],
      },
      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%B'
          }
        }
      },
    };

    $scope.work_alloted_chart = {
      bindto: '#work_alloted_chart',
      data: {
        x: 'x',
        columns: [
                  ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
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

    $scope.unmet_demand_chart = {
      bindto: '#unmet_demand_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Unmet Demand', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250]
        ],
        // type: 'spline'
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

    $scope.unemployment_allowances_chart = {
      bindto: '#unemployment_allowances_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Unemployment Allowances - Due for PDs', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],
            ['Amount payable', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],

        ],
        type: 'bar',
        types: {
          'Unemployment Allowances - Due for PDs': 'line',
          'Amount payable': 'bar',
        },
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


    $scope.persondays_yearly_chart = {
      bindto: '#persondays_yearly_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2012', '2013', '2014'],
            ['Persondays : LB Approved Yearly', 30, 200, 100],
        ],
        type: 'bar'
      },
      bar: {
        width: {
          ratio: 0.5
        }
      }
    };

    $scope.gps_by_expenditure_chart = {
      bindto: '#gps_by_expenditure_chart',
      data: {

        columns: [
            ['GPs with nil expenditure', 20],
            ['GPs with no employment generation in last month)', 30],
            ['GPs with no approved works', 10],
            ['GPs with no ongoing works', 40],
        ],
        type: 'donut'
      },
      donut: {
        title: "Total GPs",
        onclick: function(d, i) {
          console.log(d, i);
        },
        onmouseover: function(d, i) {
          console.log(d, i);
        },
        onmouseout: function(d, i) {
          console.log(d, i);
        }
      }
    };

    $scope.persondays_monthly_chart = {
      bindto: '#persondays_monthly_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Persondays : LB Approved Monthly', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250]
        ],
        // type: 'spline'
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

    $scope.persondays_pd_caste_chart = {
      bindto: '#persondays_pd_caste_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Persondays:PD Generated for SC', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],
            ['ST', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],
            ['Women', 110, 300, 120, 300, 350, 250, 100, 240, 230, 300, 230, 310],

        ],
        type: 'bar',
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


    $scope.hh_providedemployment_chart = {
      bindto: '#hh_providedemployment_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['HH provided employment', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250]
        ],
        // type: 'spline'
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

    $scope.workcompletion_chart = {
      bindto: '#workcompletion_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Work Completion Rate', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250]
        ],
        // type: 'spline'
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



    $scope.hhscompleted100days_chart = {
      bindto: '#hhscompleted100days_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['HHs completed 100 days', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250]
        ],
        // type: 'spline'
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


    $scope.wage_chart = {
      bindto: '#wage_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Total', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],
            ['Wage %', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],

        ],
        type: 'bar',
        types: {
          'Total': 'bar',
          'Wage %': 'line',
        },
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


    $scope.delayedpayment_chart = {
      bindto: '#delayedpayment_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Delayed payment :PDs Payable', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],
            ['Amount payable', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],
        ],
        type: 'bar',
        types: {
          'Delayed payment :PDs Payable': 'bar',
          'Amount payable': 'line',
        },
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


    $scope.wageperPD_chart = {
      bindto: '#wageperPD_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Average wage per PD', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],
            ['Cost per PD', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],
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


    $scope.efms_chart = {
      bindto: '#efms_chart',
      data: {

        columns: [
            ['Started in No. of blocks(wages)', 50],
            ['Started in No. of blocks(Material)', 30],
            ['Started in No. of blocks(Admin)', 20],
        ],
        type: 'donut'
      },
      donut: {
        title: "eFMS"
      }
    };

    $scope.DBT_chart = {
      bindto: '#DBT_chart',
      data: {

        columns: [
            ['Active workers A/Cs freezed', 70],
            ['Aadhar seeding against total active worker', 30],
        ],
        type: 'donut'
      },
      donut: {
        title: "DBT"
      }
    };



    //////////////////////////
    //      View Results    //
    //////////////////////////

    $scope.viewResults = function() {
      params = buildCode();

      YearlyReport.fetch(params, $scope.selectedYear).then(function(response) {
        $scope.yearlydata = response[0];
      });
      // $scope.yearlydata = YearlyReport.testfetch;
    };



  }
]);
