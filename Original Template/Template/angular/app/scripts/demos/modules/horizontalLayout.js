angular
  .module('theme.demos.horizontal_layout', [
    'theme.core.services'
  ])
  .controller('HorizontalPageController', ['$scope', '$theme', function($scope, $theme) {
    'use strict';
    var isHorizontal = $theme.get('layoutHorizontal');
    $theme.set('layoutHorizontal', true);

    $scope.$on('$destroy', function() {
      if (!isHorizontal) {
        $theme.set('layoutHorizontal', false);
      }
    });
  }])
  .controller('HorizontalPage2Controller', ['$scope', '$theme', function($scope, $theme) {
    'use strict';
    var isHorizontal = $theme.get('layoutHorizontal');
    var isLargeIcons = $theme.get('layoutHorizontalLargeIcons');
    $theme.set('layoutHorizontal', true);
    $theme.set('layoutHorizontalLargeIcons', true);

    $scope.$on('$destroy', function() {
      if (!isHorizontal) {
        $theme.set('layoutHorizontal', false);
      }
      if (!isLargeIcons) {
        $theme.set('layoutHorizontalLargeIcons', false);
      }
    });
  }]);