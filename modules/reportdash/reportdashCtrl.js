var reportdash = angular.module('ReportDash', []);

reportdash.controller('reportdashCtrl', ['$scope', '$rootScope', 'YearlyReport', 'Regions', 'GPRegions', 'MonthlyReport', 'YearlyReportNational', 'MonthlyReportNational',
  function($scope, $rootScope, YearlyReport, Regions, GPRegions, MonthlyReport, YearlyReportNational, MonthlyReportNational) {
    $scope.isTable = false;
    $scope.switchview = function() {
      $scope.isTable = !$scope.isTable;
    };

    //*****************[ Region Handelers ]*******************//

    $scope.years = ['2014-2015', '2013-2014', '2012-2013'];
    $scope.selectedYear = $scope.years[0];
    $scope.$watch('selectedYear', function() {
      $scope.selectedState = null;
      Regions.fetch($scope.selectedYear).then(function(data) {
        $scope.regions = data;

      });
    });

    $scope.$watch('selectedState', function() {
      if ($scope.regions) fetchDistricts($scope.selectedState);
    });
    $scope.$watch('selectedDistrict', function() {
      if ($scope.regions) fetchBlocks($scope.selectedState, $scope.selectedDistrict);
    });
    $scope.$watch('selectedBlock', function() {
      if ($scope.regions) fetchGPs(leftPad($scope.selectedState, 2) + leftPad($scope.selectedDistrict, 2) + leftPad($scope.selectedBlock, 3));
    });

    function fetchDistricts(selectedState) {
       console.log(selectedState);
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

    $scope.getKeyByValue = function(obje,  value ) {
        for( var prop in obje ) {
            if( obje.hasOwnProperty( prop ) ) {
                 if( obje[ prop ] == value ){
                     return prop;
                 }
            }
        }
    }



    //////////////////////////
    // Vizulization Loading //
    //////////////////////////
    $scope.demand_reg_chart = {
      bindto: '#demand_reg_chart',
      data: {
        x: 'x',
        columns: [
                  ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
                  ['Households registered demand', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                  ['Households provided employment', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
              ],
      },
      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%B'
          }
        },
        y: {
          tick: {
            format: d3.format(",")
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
                  ['Person-days generated', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                  ['Person-days projected', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
              ],
      },
      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%B'
          }
        },
        y: {
          tick: {
            format: d3.format(",")
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
            ['Person-days of unemployment due', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            ['Amount payable (Rs.)', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],

        ],
        type: 'bar',
        types: {
          'Person-days of unemployment due': 'line',
          'Amount payable (Rs.)': 'bar',
        },
      },
      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%B'
          }
        },
        y: {
          tick: {
            format: d3.format(",")
          }
        }
      }
    };



    $scope.work_category_wise_chart = {
      bindto: '#work_category_wise_chart',
      data: {

        columns: [
          ['Aanganbadi', 10],
          ['Coastal areas', 10],
          ['Drought proofing', 10],
          ['Rural drinking water', 10],
          ['Food grain', 10],
          ['Flood control protection', 10],
          ['Fisheries', 10],
          ['Micro irrigation works', 10],
          ["Works on individuals' land (Cat 4)", 10],
          ['Land development', 10],
          ['Other works', 10],
          ['Playground', 10],
          ['Rural connectivity', 10],
          ['Rural sanitation', 10],
          ['Bharat Nirman Rajiv Gandhi Seva Kendra', 10],
          ['Water conservation and harvesting', 10],
          ['Renovation of traditional water bodies', 10]
        ],
        type: 'donut'
      },
      donut: {
        title: "Total Work",
      },
      size: {
        'height': 400
      }
    };

    $scope.expenditure_category_wise_chart = {
      bindto: '#expenditure_category_wise_chart',
      data: {
        columns: [
          ['Aanganbadi', 10],
          ['Coastal areas', 10],
          ['Drought proofing', 10],
          ['Rural drinking water', 10],
          ['Food grain', 10],
          ['Flood control protection', 10],
          ['Fisheries', 10],
          ['Micro irrigation works', 10],
          ["Works on individuals' land (Cat 4)", 10],
          ['Land development', 10],
          ['Other works', 10],
          ['Playground', 10],
          ['Rural connectivity', 10],
          ['Rural sanitation', 10],
          ['Bharat Nirman Rajiv Gandhi Seva Kendra', 10],
          ['Water conservation and harvesting', 10],
          ['Renovation of traditional water bodies', 10]
        ],
        type: 'donut'
      },
      donut: {
        title: "Total Expenditure",
      },
      size: {
        'height': 400
      }
    };


    $scope.hh_providedemployment_chart = {
      bindto: '#hh_providedemployment_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Previous year', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            ['Households provided employment', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
        ],
        type: 'bar',
      },

      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%B'
          }
        },
        y: {
          tick: {
            format: d3.format(",")
          }
        }
      }
    };
    $scope.persondays_per_hh_chart = {
      bindto: '#persondays_per_hh_chart',
      data: {
        x: 'x',
        columns: [
            ['x', '2013-04-01', '2013-05-01', '2013-06-01', '2013-07-01', '2013-08-01', '2013-09-01', '2013-10-01', '2013-11-01', '2013-12-01', '2014-01-01', '2014-02-01', '2014-03-01'],
            ['Previous year', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            ['Average person-days per household', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
        ],
        type: 'bar',
      },

      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%B'
          }
        },
        y: {
          tick: {
            format: d3.format(",")
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
            ['Previous year', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            ['Wage expenditure (in lacs of Rs.)', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0]

        ],
        type: 'bar',
        types: {
          'Wage expenditure (in lacs of Rs.)': 'bar',
        },
      },
      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%B'
          }
        },
        y: {
          tick: {
            format: d3.format(",")
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
            ['Muster rolls with delayed payment', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        ],
        type: 'bar',
        types: {
          'Muster rolls with delayed payment': 'bar'
        },
      },
      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%B'
          }
        },
        y: {
          tick: {
            format: d3.format(",")
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
            ['Delay in days', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
            ['Amount payable (Rs.)', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        ],
        type: 'bar',
        types: {
          'Delay in days': 'bar',
          'Amount payable (Rs.)': 'line',
        },
      },
      axis: {
        x: {
          type: 'timeseries',
          tick: {
            format: '%B'
          }
        },
        y: {
          tick: {
            format: d3.format(",")
          }
        }
      }
    };


    //////////////////////////
    //      View Results    //
    //////////////////////////

    YearlyReportNational.fetch($scope.selectedYear).then(function(response) {
      $scope.yearlydata = response[0];
    });
    MonthlyReportNational.fetch($scope.selectedYear).then(function(response) {
      $scope.monthlydata = response[0];
      loadGraph();
    });

    $scope.viewResults = function() {
      params = buildCode();

      YearlyReport.fetch(params, $scope.selectedYear).then(function(response) {
        $scope.yearlydata = response[0];
      });
      MonthlyReport.fetch(params, $scope.selectedYear).then(function(response) {
        $scope.monthlydata = response[0];
        loadGraph();
      });
    };

    function loadGraph() {

      $scope.demand_reg_chart.data.columns[1] = [
            'Households registered demand',
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

      $scope.demand_reg_chart.data.columns[2] = [
            'Households provided employment',
            $scope.monthlydata.april_hh_P_emp,
            $scope.monthlydata.may_hh_P_emp,
            $scope.monthlydata.june_hh_P_emp,
            $scope.monthlydata.july_hh_P_emp,
            $scope.monthlydata.aug_hh_P_emp,
            $scope.monthlydata.sep_hh_P_emp,
            $scope.monthlydata.oct_hh_P_emp,
            $scope.monthlydata.nov_hh_P_emp,
            $scope.monthlydata.dec_hh_P_emp,
            $scope.monthlydata.jan_hh_P_emp,
            $scope.monthlydata.feb_hh_P_emp,
            $scope.monthlydata.march_hh_P_emp
          ];

      $scope.persondays_per_hh_chart.data.columns[1] = [
            'Previous year',
            $scope.monthlydata.apr_PD_per_hh_preyr,
            $scope.monthlydata.may_PD_per_hh_preyr,
            $scope.monthlydata.jun_PD_per_hh_preyr,
            $scope.monthlydata.jul_PD_per_hh_preyr,
            $scope.monthlydata.aug_PD_per_hh_preyr,
            $scope.monthlydata.sep_PD_per_hh_preyr,
            $scope.monthlydata.oct_PD_per_hh_preyr,
            $scope.monthlydata.nov_PD_per_hh_preyr,
            $scope.monthlydata.dec_PD_per_hh_preyr,
            $scope.monthlydata.jan_PD_per_hh_preyr,
            $scope.monthlydata.feb_PD_per_hh_preyr,
            $scope.monthlydata.march_PD_per_hh_preyr
          ];
      $scope.persondays_per_hh_chart.data.columns[2] = [
            'Average person-days per household',
            $scope.monthlydata.apr_PD_per_hh,
            $scope.monthlydata.may_PD_per_hh,
            $scope.monthlydata.jun_PD_per_hh,
            $scope.monthlydata.jul_PD_per_hh,
            $scope.monthlydata.aug_PD_per_hh,
            $scope.monthlydata.sep_PD_per_hh,
            $scope.monthlydata.oct_PD_per_hh,
            $scope.monthlydata.nov_PD_per_hh,
            $scope.monthlydata.dec_PD_per_hh,
            $scope.monthlydata.jan_PD_per_hh,
            $scope.monthlydata.feb_PD_per_hh,
            $scope.monthlydata.march_PD_per_hh
          ];



      $scope.demand_labourbudget_chart.data.columns[1] = [
            'Person-days generated',
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
            'Person-days projected',
            $scope.monthlydata.april_lb,
            $scope.monthlydata.may_lb - $scope.monthlydata.april_lb,
            $scope.monthlydata.june_lb - $scope.monthlydata.may_lb,
            $scope.monthlydata.july_lb - $scope.monthlydata.june_lb,
            $scope.monthlydata.aug_lb - $scope.monthlydata.july_lb,
            $scope.monthlydata.sep_lb - $scope.monthlydata.aug_lb,
            $scope.monthlydata.oct_lb - $scope.monthlydata.sep_lb,
            $scope.monthlydata.nov_lb - $scope.monthlydata.oct_lb,
            $scope.monthlydata.dec_lb - $scope.monthlydata.nov_lb,
            $scope.monthlydata.jan_lb - $scope.monthlydata.dec_lb,
            $scope.monthlydata.feb_lb - $scope.monthlydata.jan_lb,
            $scope.monthlydata.march_lb - $scope.monthlydata.feb_lb
          ];


      // Chart 2
      $scope.unemployment_allowances_chart.data.columns[1] = [
            'Person-days of unemployment due',
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
      $scope.unemployment_allowances_chart.data.columns[2] = [
            'Amount payable (Rs.)',
            $scope.monthlydata.april_unemp_amt,
            $scope.monthlydata.may_unemp_amt,
            $scope.monthlydata.june_unemp_amt,
            $scope.monthlydata.july_unemp_amt,
            $scope.monthlydata.aug_unemp_amt,
            $scope.monthlydata.sep_unemp_amt,
            $scope.monthlydata.oct_unemp_amt,
            $scope.monthlydata.nov_unemp_amt,
            $scope.monthlydata.dec_unemp_amt,
            $scope.monthlydata.jan_unemp_amt,
            $scope.monthlydata.feb_unemp_amt,
            $scope.monthlydata.march_unemp_amt
          ];


      $scope.work_category_wise_chart.data.columns = [
              ['Aanganbadi', $scope.monthlydata.AV_work],
              ['Coastal areas', $scope.monthlydata.CA_work],
              ['Drought proofing', $scope.monthlydata.DP_work],
              ['Rural drinking water', $scope.monthlydata.DW_work],
              ['Food grain', $scope.monthlydata.FG_work],
              ['Flood control protection', $scope.monthlydata.FP_work],
              ['Fisheries', $scope.monthlydata.FR_work],
              ['Micro irrigation works', $scope.monthlydata.IC_work],
              ["Works on individuals' land (Cat 4)", $scope.monthlydata.IF_work],
              ['Land development', $scope.monthlydata.LD_work],
              ['Other works', $scope.monthlydata.OP_work],
              ['Playground', $scope.monthlydata.PG_work],
              ['Rural connectivity', $scope.monthlydata.RC_work],
              ['Rural sanitation', $scope.monthlydata.RS_work],
              ['Bharat Nirman Rajiv Gandhi Seva Kendra', $scope.monthlydata.SK_work],
              ['Water conservation and harvesting', $scope.monthlydata.WC_work],
              ['Renovation of traditional water bodies', $scope.monthlydata.WH_work]
          ];


      $scope.expenditure_category_wise_chart.data.columns = [
              ['Aanganbadi', $scope.monthlydata.AV_exp],
              ['Coastal areas', $scope.monthlydata.CA_exp],
              ['Drought proofing', $scope.monthlydata.DP_exp],
              ['Rural drinking water', $scope.monthlydata.DW_exp],
              ['Food grain', $scope.monthlydata.FG_exp],
              ['Flood control protection', $scope.monthlydata.FP_exp],
              ['Fisheries', $scope.monthlydata.FR_exp],
              ['Micro irrigation works', $scope.monthlydata.IC_exp],
              ["Works on individuals' land (Cat 4)", $scope.monthlydata.IF_exp],
              ['Land development', $scope.monthlydata.LD_exp],
              ['Other works', $scope.monthlydata.OP_exp],
              ['Playground', $scope.monthlydata.PG_exp],
              ['Rural connectivity', $scope.monthlydata.RC_exp],
              ['Rural sanitation', $scope.monthlydata.RS_exp],
              ['Bharat Nirman Rajiv Gandhi Seva Kendra', $scope.monthlydata.SK_exp],
              ['Water conservation and harvesting', $scope.monthlydata.WC_exp],
              ['Renovation of traditional water bodies', $scope.monthlydata.WH_exp]
          ];

      $scope.hh_providedemployment_chart.data.columns[1] = [
            'Previous Year',
            $scope.monthlydata.april_hh_P_emp_pre,
            $scope.monthlydata.may_hh_P_emp_pre,
            $scope.monthlydata.june_hh_P_emp_pre,
            $scope.monthlydata.july_hh_P_emp_pre,
            $scope.monthlydata.aug_hh_P_emp_pre,
            $scope.monthlydata.sep_hh_P_emp_pre,
            $scope.monthlydata.oct_hh_P_emp_pre,
            $scope.monthlydata.nov_hh_P_emp_pre,
            $scope.monthlydata.dec_hh_P_emp_pre,
            $scope.monthlydata.jan_hh_P_emp_pre,
            $scope.monthlydata.feb_hh_P_emp_pre,
            $scope.monthlydata.march_hh_P_emp_pre
          ];
      $scope.hh_providedemployment_chart.data.columns[2] = [
            'Households provided employment',
            $scope.monthlydata.april_hh_P_emp,
            $scope.monthlydata.may_hh_P_emp,
            $scope.monthlydata.june_hh_P_emp,
            $scope.monthlydata.july_hh_P_emp,
            $scope.monthlydata.aug_hh_P_emp,
            $scope.monthlydata.sep_hh_P_emp,
            $scope.monthlydata.oct_hh_P_emp,
            $scope.monthlydata.nov_hh_P_emp,
            $scope.monthlydata.dec_hh_P_emp,
            $scope.monthlydata.jan_hh_P_emp,
            $scope.monthlydata.feb_hh_P_emp,
            $scope.monthlydata.march_hh_P_emp
          ];

      $scope.wage_expenditure_chart.data.columns[1] = [
            'Previous Year',
            $scope.monthlydata.apr_lab_pre,
            $scope.monthlydata.may_lab_pre,
            $scope.monthlydata.jun_lab_pre,
            $scope.monthlydata.jul_lab_pre,
            $scope.monthlydata.aug_lab_pre,
            $scope.monthlydata.sep_lab_pre,
            $scope.monthlydata.oct_lab_pre,
            $scope.monthlydata.nov_lab_pre,
            $scope.monthlydata.dec_lab_pre,
            $scope.monthlydata.jan_lab_pre,
            $scope.monthlydata.feb_lab_pre,
            $scope.monthlydata.march_lab_pre
          ];
      $scope.wage_expenditure_chart.data.columns[2] = [
            'Wage expenditure (in lacs of Rs.)',
            $scope.monthlydata.apr_lab,
            $scope.monthlydata.may_lab,
            $scope.monthlydata.jun_lab,
            $scope.monthlydata.jul_lab,
            $scope.monthlydata.aug_lab,
            $scope.monthlydata.sep_lab,
            $scope.monthlydata.oct_lab,
            $scope.monthlydata.nov_lab,
            $scope.monthlydata.dec_lab,
            $scope.monthlydata.jan_lab,
            $scope.monthlydata.feb_lab,
            $scope.monthlydata.march_lab
          ];
      $scope.unpaid_delay_chart.data.columns[1] = [
            'Muster rolls with delayed payment',
            $scope.monthlydata.april_unpaid_delay,
            $scope.monthlydata.may_unpaid_delay,
            $scope.monthlydata.june_unpaid_delay,
            $scope.monthlydata.july_unpaid_delay,
            $scope.monthlydata.aug_unpaid_delay,
            $scope.monthlydata.sep_unpaid_delay,
            $scope.monthlydata.oct_unpaid_delay,
            $scope.monthlydata.nov_unpaid_delay,
            $scope.monthlydata.dec_unpaid_delay,
            $scope.monthlydata.jan_unpaid_delay,
            $scope.monthlydata.feb_unpaid_delay,
            $scope.monthlydata.march_unpaid_delay
          ];


      $scope.delayedpayment_chart.data.columns[1] = [
            'Delay in days',
            $scope.monthlydata.april_delay,
            $scope.monthlydata.may_delay,
            $scope.monthlydata.june_delay,
            $scope.monthlydata.july_delay,
            $scope.monthlydata.aug_delay,
            $scope.monthlydata.sep_delay,
            $scope.monthlydata.oct_delay,
            $scope.monthlydata.nov_delay,
            $scope.monthlydata.dec_delay,
            $scope.monthlydata.jan_delay,
            $scope.monthlydata.feb_delay,
            $scope.monthlydata.march_delay
          ];
      $scope.delayedpayment_chart.data.columns[2] = [
            'Amount payable (Rs.)',
            $scope.monthlydata.april_delay_amt,
            $scope.monthlydata.may_delay_amt,
            $scope.monthlydata.june_delay_amt,
            $scope.monthlydata.july_delay_amt,
            $scope.monthlydata.aug_delay_amt,
            $scope.monthlydata.sep_delay_amt,
            $scope.monthlydata.oct_delay_amt,
            $scope.monthlydata.nov_delay_amt,
            $scope.monthlydata.dec_delay_amt,
            $scope.monthlydata.jan_delay_amt,
            $scope.monthlydata.feb_delay_amt,
            $scope.monthlydata.march_delay_amt
          ];
    }



      }
        ]);
