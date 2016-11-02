angular
  .module('theme.calendar', [])
  .directive('makeFullCalendar', ['$window', '$theme', function($window, $theme) {
    'use strict';
    return {
      restrict: 'A',
      scope: {
        options: '=makeFullCalendar',
        events: '=ngModel'
      },
      link: function(scope, element) {
        var calendar = {};
        var defaultOptions = angular.extend({
          header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
          },
          editable: true,
          eventLimit: true,
          events: scope.options.events,
          buttonText: {
            today:    'Today',
            month:    'Month',
            week:     'Week',
            day:      'Day'
          },
          buttonIcons: {
              prev: 'fa fa fa-angle-left',
              next: 'fa fa fa-angle-right',
              prevYear: 'fa fa fa-angle-double-left',
              nextYear: 'fa fa fa-angle-double-left'
          }
        }, scope.options);

        if (defaultOptions.droppable === true) {
          defaultOptions.drop = function(date, allDay) {
            var originalEventObject = angular.element(this).data('eventObject');
            var copiedEventObject = angular.element.extend({}, originalEventObject);
            copiedEventObject.start = date;
            copiedEventObject.allDay = allDay;
            calendar.fullCalendar('renderEvent', copiedEventObject, true);
            if (defaultOptions.removeDroppedEvent === true) {
              angular.element(this).remove();
            }
          };
        }

        calendar = $(element).html('').fullCalendar(defaultOptions);
      }
    }
  }])
  .directive('draggableEvent', function() {
    'use strict';
    return {
      restrict: 'A',
      scope: {
        eventDef: '=draggableEvent'
      },
      link: function(scope, element) {
        angular.element(element).draggable({
          zIndex: 999,
          revert: true,
          revertDuration: 0
        });
        angular.element(element).data('eventObject', scope.eventDef);
      }
    };
  });