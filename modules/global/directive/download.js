reportdash.directive('downloadchart', function() {
  return {
    restrict: 'AE',
    replace: false,
    template: '<button class="btn btn-success" ng-click="downloadChart()">Download Chart</button>',
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {

        $scope.downloadChart = function() {
          svgenie.save('test', {
            name: 'xy.png'
          })
        };


      }
    ],
    scope: {
      elename: '='
    },
    link: function(scope, el, attrs) {}
  };
});
