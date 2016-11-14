angular
  .module('theme.demos.not_found', [
    'theme.core.services'
  ])
  .controller('NotFoundController', ['$scope', '$theme', function($scope, $theme) {
    'use strict';
    $theme.set('fullscreen', true);

    $scope.$on('$destroy', function() {
      $theme.set('fullscreen', false);
    });
  }]);