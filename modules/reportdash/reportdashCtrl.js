var reportdash = angular.module('ReportDash', []);

reportdash.controller('reportdashCtrl', ['$scope', '$rootScope', 'YearlyReport', 'Regions', 'GPRegions', 'MonthlyReport',
  function($scope, $rootScope, YearlyReport, Regions, GPRegions,MonthlyReport) {
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
        fetchGPs(leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2) + leftPad($scope.selectedBlock, 3));
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
          code: leftPad($scope.selectedState , 2),
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

    //////////////////////////
    // Vizulization Loading //
    //////////////////////////
    $scope.demand_reg_chart = {
      bindto: '#demand_reg_chart',
      data: {
        x: 'x',
        columns: [
                  ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
                  ['Demand Registered', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
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


    $scope.demand_labourbudget_chart = {
      bindto: '#demand_labourbudget_chart',
      data: {
        x: 'x',
        columns: [
                  ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
                  ['Persondays Generated', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                  ['Projected Persondays', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
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


    $scope.unemployment_allowances_chart = {
      bindto: '#unemployment_allowances_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Unemployment Allowances - Due for PDs',  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            ['Amount payable',  0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],

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



    $scope.work_category_wise_chart = {
      bindto: '#work_category_wise_chart',
      data: {

        columns: [
            ['Aanganbadi', 0],
            ['Coastal Areas',0],
            ['Dourght Proofing', 0],
            ['Rural Drinking Water', 0],
            ['Food Grain', 0],
            ['Flood Control and Protection', 0],
            ['Fishries', 0],
            ['Micro Irrigation Works', 0],
            ['Works on individuals land(Cat 4)', 0],
            ['Land Development', 0],
            ['Other Works', 0],
            ['Playground', 0],
            ['Rural Connectivity', 0],
            ['Rural Sanitation', 0],
            ['Bharat Nirman Rajiv Gandhi Seva Kendra', 0],
            ['Water Conservation and Harvesting', 0],
            ['Renovation of traditional Water Bodies', 0]
        ],
        type: 'donut'
      },
      donut: {
        title: "Total Work",
      }
    };

    $scope.expenditure_category_wise_chart = {
      bindto: '#expenditure_category_wise_chart',
      data: {

        columns: [
            ['Aanganbadi', 0],
            ['Coastal Areas',0],
            ['Dourght Proofing', 0],
            ['Rural Drinking Water', 0],
            ['Food Grain', 0],
            ['Flood Control and Protection', 0],
            ['Fishries', 0],
            ['Micro Irrigation Works', 0],
            ['Works on individuals land(Cat 4)', 0],
            ['Land Development', 0],
            ['Other Works', 0],
            ['Playground', 0],
            ['Rural Connectivity', 0],
            ['Rural Sanitation', 0],
            ['Bharat Nirman Rajiv Gandhi Seva Kendra', 0],
            ['Water Conservation and Harvesting', 0],
            ['Renovation of traditional Water Bodies', 0]
        ],
        type: 'donut'
      },
      donut: {
        title: "Total Expenditure",
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

    $scope.wage_expenditure_chart = {
      bindto: '#wage_expenditure_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Wage Expenditure (In Lacs)', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],

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

    $scope.unpaid_delay_chart = {
      bindto: '#unpaid_delay_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Unpaid Delay', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        ],
        type: 'bar',
        types: {
          'Unpaid Delay': 'bar'
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

    $scope.DBT_chart = {
      bindto: '#DBT_chart',
      data: {

        columns: [
            ['Active workers A/Cs freezed', 0],
            ['Aadhar seeding against total active worker', 0],
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
      MonthlyReport.fetch(params, $scope.selectedYear).then(function(response) {
        $scope.monthlydata = response[0];

        $scope.demand_reg_chart.data.columns[1] = [
          'Demand Registered', 
          $scope.monthlydata.april_demand_reg, 
          $scope.monthlydata.may_demand_reg,
          $scope.monthlydata.june_demand_reg, 
          $scope.monthlydata.july_demand_reg, 
          $scope.monthlydata.aug_demand_reg,
          $scope.monthlydata.sep_demand_reg,       
          $scope.monthlydata.oct_demand_reg, 
          $scope.monthlydata.nov_demand_reg, 
          $scope.monthlydata.dec_demand_reg, 
          $scope.monthlydata.jan_demand_reg,
          $scope.monthlydata.feb_demand_reg, 
          $scope.monthlydata.march_demand_reg
        ];


        $scope.demand_labourbudget_chart.data.columns[1] = [
          'Persondays Generated', 
          $scope.monthlydata.april_work_allot, 
          $scope.monthlydata.may_work_allot,
          $scope.monthlydata.june_work_allot, 
          $scope.monthlydata.july_work_allot, 
          $scope.monthlydata.aug_work_allot,
          $scope.monthlydata.sep_work_allot,       
          $scope.monthlydata.oct_work_allot, 
          $scope.monthlydata.nov_work_allot, 
          $scope.monthlydata.dec_work_allot, 
          $scope.monthlydata.jan_work_allot,
          $scope.monthlydata.feb_work_allot, 
          $scope.monthlydata.march_work_allot
        ];
        $scope.demand_labourbudget_chart.data.columns[2] = [
          'Projected Persondays', 
          $scope.monthlydata.april_lb, 
          $scope.monthlydata.may_lb-$scope.monthlydata.april_lb,
          $scope.monthlydata.june_lb-$scope.monthlydata.may_lb, 
          $scope.monthlydata.july_lb-$scope.monthlydata.june_lb, 
          $scope.monthlydata.aug_lb-$scope.monthlydata.july_lb,
          $scope.monthlydata.sep_lb-$scope.monthlydata.aug_lb,       
          $scope.monthlydata.oct_lb-$scope.monthlydata.sep_lb, 
          $scope.monthlydata.nov_lb-$scope.monthlydata.oct_lb, 
          $scope.monthlydata.dec_lb-$scope.monthlydata.nov_lb, 
          $scope.monthlydata.jan_lb-$scope.monthlydata.dec_lb,
          $scope.monthlydata.feb_lb-$scope.monthlydata.jan_lb, 
          $scope.monthlydata.march_lb-$scope.monthlydata.feb_lb
        ];


        // Chart 2
        $scope.unemployment_allowances_chart.data.columns[1] = [
          'Unemployment Allowances - Due for PDs', 
          $scope.monthlydata.april_unemp, 
          $scope.monthlydata.may_unemp,
          $scope.monthlydata.june_unemp, 
          $scope.monthlydata.july_unemp, 
          $scope.monthlydata.aug_unemp,
          $scope.monthlydata.sep_unemp,       
          $scope.monthlydata.oct_unemp, 
          $scope.monthlydata.nov_unemp, 
          $scope.monthlydata.dec_unemp, 
          $scope.monthlydata.jan_unemp,
          $scope.monthlydata.feb_unemp, 
          $scope.monthlydata.march_unemp
        ];   


        $scope.work_category_wise_chart.data.columns= [
            ['Aanganbadi', $scope.monthlydata.AV_work],
            ['Coastal Areas',$scope.monthlydata.CA_work],
            ['Dourght Proofing', $scope.monthlydata.DP_work],
            ['Rural Drinking Water', $scope.monthlydata.DW_work],
            ['Food Grain', $scope.monthlydata.FG_work],
            ['Flood Control and Protection', $scope.monthlydata.FP_work],
            ['Fishries', $scope.monthlydata.FR_work],
            ['Micro Irrigation Works', $scope.monthlydata.IC_work],
            ['Works on individuals land(Cat 4)', $scope.monthlydata.IF_work],
            ['Land Development', $scope.monthlydata.LD_work],
            ['Other Works', $scope.monthlydata.OP_work],
            ['Playground', $scope.monthlydata.PG_work],
            ['Rural Connectivity', $scope.monthlydata.RC_work],
            ['Rural Sanitation', $scope.monthlydata.RS_work],
            ['Bharat Nirman Rajiv Gandhi Seva Kendra', $scope.monthlydata.SK_work],
            ['Water Conservation and Harvesting', $scope.monthlydata.WC_work],
            ['Renovation of traditional Water Bodies', $scope.monthlydata.WH_work]
        ]; 


        $scope.expenditure_category_wise_chart.data.columns= [
            ['Aanganbadi', $scope.monthlydata.AV_exp],
            ['Coastal Areas',$scope.monthlydata.CA_exp],
            ['Dourght Proofing', $scope.monthlydata.DP_exp],
            ['Rural Drinking Water', $scope.monthlydata.DW_exp],
            ['Food Grain', $scope.monthlydata.FG_exp],
            ['Flood Control and Protection', $scope.monthlydata.FP_exp],
            ['Fishries', $scope.monthlydata.FR_exp],
            ['Micro Irrigation Works', $scope.monthlydata.IC_exp],
            ['Works on individuals land(Cat 4)', $scope.monthlydata.IF_exp],
            ['Land Development', $scope.monthlydata.LD_exp],
            ['Other Works', $scope.monthlydata.OP_exp],
            ['Playground', $scope.monthlydata.PG_exp],
            ['Rural Connectivity', $scope.monthlydata.RC_exp],
            ['Rural Sanitation', $scope.monthlydata.RS_exp],
            ['Bharat Nirman Rajiv Gandhi Seva Kendra', $scope.monthlydata.SK_exp],
            ['Water Conservation and Harvesting', $scope.monthlydata.WC_exp],
            ['Renovation of traditional Water Bodies', $scope.monthlydata.WH_exp]
        ];

        $scope.DBT_chart.data.columns= [
            ['Active workers A/Cs freezed',  $scope.yearlydata.frez_act_pers],
            ['Aadhar seeding against total active worker',  $scope.yearlydata.yearlydata.aadhaar_seedpers],
        ];






      });
    };



  }
]);
