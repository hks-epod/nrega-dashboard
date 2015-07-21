reportdash.directive('downloadchart', function() {
  return {
    restrict: 'AE',
    replace: false,
    template: '<button class="btn btn-sm btn-default pull-right " ng-click="downloadChart()"><span class="glyphicon glyphicon-floppy-save"></span> Save</button>',
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {

        $scope.downloadChart = function() {

          idtext= '#'+$scope.downid+ ' svg';

          $(idtext).inlinify();

          svgenie.save($scope.downid, {
            name: $scope.downid+'chart.png'
          })


        };


      }
    ],
    scope: {
            elename: '@elename'
        },
    link: function(scope, el, attrs) {

      scope.downid = attrs["elename"];

      $.fn.inlinify = function() {

        var rules, i, j, len, sheets, nodes, parent;
        sheets = document.styleSheets;
        // nodes should be children and actual node itself
        nodes = $.merge(this.find('*'), this)
        // No sheets so just return
        if (!sheets)
          return;
        for (i = sheets.length - 1; i >= 1; i--) {
          try {
            rules = sheets[i].cssRules || sheets[i].rules;
          } catch (err) {
            // Skip over external stylesheets in FF
            if (err.name === 'SecurityError') {
              console.log('Skipping cross-domain stylesheet');
              continue;
            } else {
              throw err;
            }
          }
          // No rules for sheet so continue
          if (!rules)
            continue;

          for (j = 0, len = rules.length; j < len; j++) {

            // Skip if hover style or no selectText
            if (!rules[j].selectorText || rules[j].selectorText.indexOf("hover") !== -1)
              continue;

            $ele = $(rules[j].selectorText);

            $ele.each(function(i, elem) {
              if (nodes.index(elem) !== -1) {
                elem.style.cssText = rules[j].style.cssText + elem.style.cssText;
              }
            });
          }
        }

      };





    }
  };
});
