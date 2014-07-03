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
    $scope.demand_labourbudget_chart = {
      bindto: '#demand_labourbudget_chart',
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

    $scope.work_alloted_chart = {
      bindto: '#work_alloted_chart',
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

    $scope.unmet_demand_chart = {
      bindto: '#unmet_demand_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
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
        title: "eFMS",
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
        title: "DBT",
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
