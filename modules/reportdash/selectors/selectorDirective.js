reportdash.directive('selectyear', function() {
  return {
    restrict: 'AE',
    template: '<select class="selector" ui-select2 ng-model="selectedYear" data-placeholder="Select Year"><option value=""></option><option ng-repeat="year in years" value="{{year}}">{{year}}</option></select>',
    replace: true
  };
});

reportdash.directive('selectstate', function() {
  return {
    restrict: 'AE',
    template: '<select class="selector" ui-select2 ng-model="selectedState" data-placeholder="Select State"><option value=""></option><option ng-repeat="(key, value) in regions[0]" value="{{key}}">{{value}}</option></select>',
    replace: true
  };
});


reportdash.directive('selectdistrict', function() {
  return {
    restrict: 'AE',
    template: '<select class="selector" ui-select2 ng-model="selectedDistrict" data-placeholder="Select District"><option value=""></option><option ng-repeat="(key,val) in districts" value="{{key}}">{{val}}</option></select>',
    replace: true
  };
});


reportdash.directive('selectblock', function() {
  return {
    restrict: 'AE',
    template: '<select class="selector" ui-select2 ng-model="selectedBlock" data-placeholder="Select Block"><option value=""></option><option ng-repeat="(key, value) in blocks" value="{{key}}">{{value}}</option></select>',
    replace: true
  };
});


reportdash.directive('selectgp', function() {
  return {
    restrict: 'AE',
    template: '<select class="selector" ui-select2 ng-model="selectedGP" data-placeholder="Select GP"><option value=""></option><option ng-repeat="(key, value) in gps" value="{{key}}">{{value}}</option></select>',
    replace: true
  };
});
