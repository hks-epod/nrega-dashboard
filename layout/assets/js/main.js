//Demand Registration

var chart = c3.generate({
  data: {
    x: 'x',
    columns: [
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
            ['Demand Registered', 30, 200, 100, 400, 150, 250,30, 200, 100, 400, 150, 250],
            ['Work Allotted', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],
            ['Unmet Demand', 100, 300, 170, 400, 200, 300,100, 300, 170, 400, 200, 300],
            ['Unemplyment Allowances : Due for PDs', 90, 200, 70, 300, 100, 200,80, 200, 150, 200, 100, 100],
            ['Unemplyment Allowances : Amount Payable', 70, 100, 170, 200, 230, 140,60, 120, 100, 240, 170, 200]
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
});
