angular.module('theme.vector_maps', [])
  .directive('jvectormap', ['$timeout', function($timeout) {
    'use strict';
    return {
      restrict: 'A',
      scope: {
        options: '=',
      },
      link: function(scope, element) {
        $timeout(function() {
          element.vectorMap(scope.options);
        });
      }
    };
  }]);