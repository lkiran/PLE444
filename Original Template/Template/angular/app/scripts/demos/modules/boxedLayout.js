angular
  .module('theme.demos.boxed_layout', [
    'theme.core.services'
  ])
  .controller('BoxedPageController', ['$scope', '$theme', function($scope, $theme) {
    'use strict';
    $theme.set('layoutBoxed', true);

    $scope.$on('$destroy', function() {
      $theme.set('layoutBoxed', false);
    });
  }]);