angular
  .module('theme.chart.morris', [])
  .directive('svgChart', ['$window', function($window) {
    'use strict';
    return {
      restrict: 'EA',
      scope: {
        options: '=svgChart',
        type: '=',
      },
      link: function(scope, element, attr) {
        if ($window.Morris) {
          var elementId;
          if (!angular.element(element).attr('id')) {
            elementId = angular.element(element).attr('id', scope.type + attr.svgChart);
          } else {
            elementId = angular.element(element).attr('id');
          }
          $window.Morris[scope.type](angular.extend(scope.options, {
            element: elementId
          }));
        }
      }
    };
  }]);