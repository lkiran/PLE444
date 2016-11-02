angular
  .module('theme.chart.canvas', [])
  .directive('canvasChart', ['$window', function($window) {
    'use strict';
    return {
      restrict: 'EA',
      scope: {
        data: '=canvasChart',
        options: '=options',
        type: '=',
      },
      link: function(scope, element) {
        if ($window.Chart) {
          // console.log(element[0].getContext);
          (new $window.Chart(angular.element(element)[0].getContext('2d')))[scope.type](scope.data, scope.options);
        }
      }
    };
  }]);