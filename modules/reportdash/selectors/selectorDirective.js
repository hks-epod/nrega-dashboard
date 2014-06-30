reportdash.directive('selectstate', function() {
  return {
    restrict: 'AE',
    template: '<select class="selector" ui-select2 ng-model="selectedState" data-placeholder="Select State"><option value=""></option><option ng-repeat="(key, value) in regions[0]" value="{{key}}">{{value}}</option></select>',
    replace: true,
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {}
    ],
    link: function(scope, el, attrs) {}
  };
});


reportdash.directive('selectdistrict', function() {
  return {
    restrict: 'AE',
    template: '<select class="selector" ui-select2 ng-model="selectDistrict" data-placeholder="Select District"><option value=""></option><option ng-repeat="(key, value) in districts" value="{{key}}">{{value}}</option></select>',
    replace: true,
    controller: ['$rootScope', '$scope',
      function($rootScope, $scope) {

        // All functions here

      }
    ],
    link: function(scope, el, attrs) {
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


reportdash.directive('selectyear', function() {
  return {
    restrict: 'AE',
    template: '<select class="selector" ui-select2 ng-model="selectDistrict" data-placeholder="Select Year"><option value=""></option><option ng-repeat="year in years" value="{{$index}}">{{year}}</option></select>',
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
