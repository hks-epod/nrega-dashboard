reportdash.directive('c3chart', function() {
  return {
    restrict: 'AE',
    replace: false,
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {}
    ],
    scope: {
      vizconfig: '='
    },
    link: function(scope, el, attrs) {
      var chart=c3.generate(scope.vizconfig);
      scope.$watch('vizconfig', function(newVizconfig, oldVizconfig) {
        console.log('watching');
        chart.load({columns:newVizconfig.data.columns});
      }, true);
    }
  };
});
