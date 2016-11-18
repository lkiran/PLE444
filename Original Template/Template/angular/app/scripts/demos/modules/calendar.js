angular.module('theme.demos.calendar', [
    'theme.core.services',
    'theme.calendar'
  ])
  .config(['$routeProvider', function($routeProvider) {
    'use strict';
    $routeProvider
      .when('/calendar', {
        templateUrl: 'views/calendar.html',
        resolve: {
          loadCalendar: ['$ocLazyLoad', function($ocLazyLoad) {
            return $ocLazyLoad.load([
              'bower_components/fullcalendar/dist/fullcalendar.js'
            ]);
          }]
        }
      });
  }])
  .controller('CalendarController', ['$scope', '$theme', function($scope, $theme) {
    'use strict';
    var date = new Date();
    var d = date.getDate();
    var m = date.getMonth();
    var y = date.getFullYear();

    $scope.demoEvents = [{
      title: 'All Day Event',
      start: new Date(y, m, 8),
      backgroundColor: $theme.getBrandColor('warning')
    }, {
      title: 'Long Event',
      start: new Date(y, m, d - 5),
      end: new Date(y, m, d - 2),
      backgroundColor: $theme.getBrandColor('success')
    }, {
      id: 999,
      title: 'Repeating Event',
      start: new Date(y, m, d - 3, 16, 0),
      allDay: false,
      backgroundColor: $theme.getBrandColor('primary')
    }, {
      id: 999,
      title: 'Repeating Event',
      start: new Date(y, m, d + 4, 16, 0),
      allDay: false,
      backgroundColor: $theme.getBrandColor('danger')
    }, {
      title: 'Meeting',
      start: new Date(y, m, d, 10, 30),
      allDay: false,
      backgroundColor: $theme.getBrandColor('info')
    }, {
      title: 'Lunch',
      start: new Date(y, m, d, 12, 0),
      end: new Date(y, m, d, 14, 0),
      allDay: false,
      backgroundColor: $theme.getBrandColor('midnightblue')
    }, {
      title: 'Birthday Party',
      start: new Date(y, m, d + 1, 19, 0),
      end: new Date(y, m, d + 1, 22, 30),
      allDay: false,
      backgroundColor: $theme.getBrandColor('primary')
    }, {
      title: 'Click for Google',
      start: new Date(y, m, 28),
      end: new Date(y, m, 29),
      url: 'http://google.com/',
      backgroundColor: $theme.getBrandColor('warning')
    }];

    $scope.events = [{
      title: 'Demo Event 1'
    }, {
      title: 'Demo Event 2'
    }, {
      title: 'Demo Event 2'
    }];
    $scope.addEvent = function() {
      $scope.events.push({
        title: $scope.newEvent
      });
      $scope.newEvent = '';
    };
  }]);