//Demand Registration

var chart = c3.generate({
  bindto: '#chart',
  data: {
    x: 'x',
    columns: [
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
            ['Demand Registered', 30, 200, 100, 400, 150, 250,30, 200, 100, 400, 150, 250],
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
            ['Work Alloted', 30, 200, 100, 400, 150, 250,30, 200, 100, 400, 150, 250]
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
            ['Unmet Demand', 30, 200, 100, 400, 150, 250,30, 200, 100, 400, 150, 250]
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

var Unemployment_allowances_chart= c3.generate({
  bindto: '#unmet_Demand_chart',
  data: {
    x: 'x',
    columns: [
            ['x', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'],
            ['Due for PDs', 30, 200, 100, 400, 150, 250,30, 200, 100, 400, 150, 250],
            ['Amount payable', 130, 340, 200, 500, 250, 350, 130, 340, 200, 500, 250, 350],

        ],
    type: 'bar',
    types: {
            'Due for PDs': 'line',
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






var phy_chart = c3.generate({
    bindto: '#phy_chart',
    data: {
        columns: [
            ['HHs provided employment', 30, 200, 100, 400, 150, 250],
            ['Work completion rate', 130, 100, 140, 200, 150, 50],
            ['Total GPs', 230, 320, 210, 130, 250, 150],
            ['GPs with nil expenditure', 100, 60, 90, 250, 100, 20]
        ],
        type: 'bar'
    },
    bar: {
        width: {
            ratio: 0.5 // this makes bar width 50% of length between ticks
        }
        // or
        //width: 100 // this makes bar width 100px
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
        type : 'donut'
    },
    donut: {
        title: "eFMS",
        onclick: function (d, i) { console.log(d, i); },
        onmouseover: function (d, i) { console.log(d, i); },
        onmouseout: function (d, i) { console.log(d, i); }
    }
});

