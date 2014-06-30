reportdash.directive('selectyear', function() {
  return {
    restrict: 'AE',
    template: '<input id="rd-year" class="" ui-select2="yearselector" ng-model="selectedYear">',
    replace: true,
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {

        // All functions here

      }
    ],
    link: function(scope, el, attrs) {
      el.on('change', function(e) {

        if (e.added) {
          Tags.primary.coll = e.added.cat;
          Tags.primary.id = e.added._id;
          Tags.primary.text = e.added.text;
        }
      });
    }
  };
});


reportdash.directive('selectdistrict', function() {
  return {
    restrict: 'AE',
    template: '<input id="rd-district" class="" ui-select2="districtselector" ng-model="selectedDistrict">',
    replace: true,
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {

        // All functions here

      }
    ],
    link: function(scope, el, attrs) {
      el.on('change', function(e) {

        if (e.added) {
          Tags.primary.coll = e.added.cat;
          Tags.primary.id = e.added._id;
          Tags.primary.text = e.added.text;
        }
      });
    }
  };
});


reportdash.directive('selectblock', function() {
  return {
    restrict: 'AE',
    template: '<input id="rd-block" class="" ui-select2="blockselector" ng-model="selectedBlock">',
    replace: true,
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {

        // All functions here

      }
    ],
    link: function(scope, el, attrs) {
      el.on('change', function(e) {

        if (e.added) {
          Tags.primary.coll = e.added.cat;
          Tags.primary.id = e.added._id;
          Tags.primary.text = e.added.text;
        }
      });
    }
  };
});


reportdash.directive('selectgp', function() {
  return {
    restrict: 'AE',
    template: '<input id="rd-gp" class="" ui-select2="gpselector" ng-model="selectedGp">',
    replace: true,
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {

        // All functions here

      }
    ],
    link: function(scope, el, attrs) {
      el.on('change', function(e) {

        if (e.added) {
          Tags.primary.coll = e.added.cat;
          Tags.primary.id = e.added._id;
          Tags.primary.text = e.added.text;
        }
      });
    }
  };
});


reportdash.directive('selectstate', function() {
  return {
    restrict: 'AE',
    template: '<input id="rd-state" class="" ui-select2="stateselector" ng-model="selectedState">',
    replace: true,
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {

        // All functions here

      }
    ],
    link: function(scope, el, attrs) {
      el.on('change', function(e) {

        if (e.added) {
          Tags.primary.coll = e.added.cat;
          Tags.primary.id = e.added._id;
          Tags.primary.text = e.added.text;
        }
      });
    }
  };
});
