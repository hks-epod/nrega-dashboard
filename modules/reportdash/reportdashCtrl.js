var reportdash = angular.module('ReportDash', []);

reportdash.controller('reportdashCtrl', ['$scope', '$rootScope','YearlyReport',
  function($scope, $rootScope, YearlyReport) {
    
  	
  	// Select Year
    $scope.yearselector = {
      minimumInputLength: 1,
      placeholder: 'Select Year',
      maximumSelectionSize: 1,
      query: function(query) {
 
      },
      formatResult: function(item) {
        return '<div>' +
          '<span class="search-results-name">' + item.text + '</span >'+
          '</div>';
      }
    };

  	// Select State
    $scope.stateselector = {
      minimumInputLength: 1,
      placeholder: 'Select State',
      maximumSelectionSize: 1,
      query: function(query) {
 
      },
      formatResult: function(item) {
        return '<div>' +
          '<span class="search-results-name">' + item.text + '</span >'+
          '</div>';
      }
    };

    // Select District
    $scope.districtselector = {
      minimumInputLength: 1,
      placeholder: 'Select District',
      maximumSelectionSize: 1,
      query: function(query) {
 
      },
      formatResult: function(item) {
        return '<div>' +
          '<span class="search-results-name">' + item.text + '</span >'+
          '</div>';
      }
    };

    // Select Block
    $scope.blockselector = {
      minimumInputLength: 1,
      placeholder: 'Select Block',
      maximumSelectionSize: 1,
      query: function(query) {
 
      },
      formatResult: function(item) {
        return '<div>' +
          '<span class="search-results-name">' + item.text + '</span >'+
          '</div>';
      }
    };

    // Select GP
    $scope.gpselector = {
      minimumInputLength: 1,
      placeholder: 'Select GP',
      maximumSelectionSize: 1,
      query: function(query) {
 
      },
      formatResult: function(item) {
        return '<div>' +
          '<span class="search-results-name">' + item.text + '</span >'+
          '</div>';
      }
    };









    YearlyReport.fetch().then(function(response){
      $scope.yearlydata= response;
    });






  }
]);
