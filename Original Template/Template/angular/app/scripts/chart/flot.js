angular
  .module('theme.chart.flot', [])
  .directive('flotChart', function() {
    'use strict';
    return {
      restrict: 'AE',
      scope: {
        data: '=flotData',
        options: '=flotOptions',
        plothover: '&plotHover',
        plotclick: '&plotClick'
      },
      link: function(scope, element) {
        var plot = angular.element.plot(angular.element(element), scope.data, scope.options);

        angular.element(element).bind('plothover', function(event, position, item) {
          scope.plothover({
            event: event,
            position: position,
            item: item
          });
        });

        angular.element(element).bind('plotclick', function(event, position, item) {
          scope.plotclick({
            event: event,
            position: position,
            item: item
          });
        });

        scope.$watch('data', function(newVal) {
          plot.setData(newVal);
          plot.setupGrid();
          plot.draw();
        });

        scope.$watch('options', function(newVal) {
          plot = angular.element.plot(angular.element(element), scope.data, newVal);
        }, true);
      }
    };
  });