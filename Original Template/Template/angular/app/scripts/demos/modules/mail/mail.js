angular.module('theme.demos.mail', [
    'theme.mail.inbox_controller',
    'theme.mail.compose_controller'
  ])
  .config(['$routeProvider', function($routeProvider) {
    'use strict';
    $routeProvider
      .when('/inbox', {
        templateUrl: 'views/extras-inbox.html'
      })
      .when('/compose-mail', {
        templateUrl: 'views/extras-inbox-compose.html'
      })
      .when('/read-mail', {
        templateUrl: 'views/extras-inbox-read.html'
      });
  }]);