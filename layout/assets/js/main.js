//Demand Registration

var chart = c3.generate({
  bindto: '#chart',
  data: {
    x: 'x',
    columns: [
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
            ['Demand Registered', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250],
            ['Labour Budget', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],
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

var work_alloted_chart = c3.generate({
  bindto: '#work_alloted_chart',
  data: {
    x: 'x',
    columns: [
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
            ['Work Alloted', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250]
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

var unmet_Demand_chart = c3.generate({
  bindto: '#unmet_Demand_chart',
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
});

var Unemployment_allowances_chart = c3.generate({
  bindto: '#unmet_Demand_chart',
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
});

// Physical Charts 
var phy_chart = c3.generate({
  bindto: '#phy_chart',
  data: {
    x:'x',
    columns: [
            ['x', '2012', '2013', '2014'],
            ['Persondays Yearly', 30, 200, 100],
        ],
    type: 'bar'
  },
  bar: {
    width: {
      ratio: 0.5
    }
  }
});



var persondaysmonthly_chart = c3.generate({
  bindto: '#persondaysmonthly_chart',
  data: {
    x: 'x',
    columns: [
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
            ['Persondays Monthly', 30, 200, 100, 400, 150, 250, 30, 200, 100, 400, 150, 250]
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






var chart = c3.generate({
  bindto: '#fin_chart',
  data: {

    columns: [
            ['Started in No. of blocks(wages)', 120],
            ['Started in No. of blocks(Material)', 110],
            ['Started in No. of blocks(Admin)', 110],
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
});
