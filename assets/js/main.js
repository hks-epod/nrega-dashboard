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
});



var persondaysmonthly_chart = c3.generate({
  bindto: '#persondaysmonthly_chart',
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
});


var persondayscaste_chart = c3.generate({
  bindto: '#persondayscaste_chart',
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
});


var HHprovidedemployment_chart = c3.generate({
  bindto: '#HHprovidedemployment_chart',
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
});

var workcompletion_chart = c3.generate({
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
});

var totalgps_chart = c3.generate({
  bindto: '#totalgps_chart',
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
});


var hhscompleted100days_chart = c3.generate({
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
});


var wage_chart = c3.generate({
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
});


var delayedpayment_chart = c3.generate({
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
});


var wageperPD_chart = c3.generate({
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
});







var chart = c3.generate({
  bindto: '#fin_chart',
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
});

var DBT_chart = c3.generate({
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
});
