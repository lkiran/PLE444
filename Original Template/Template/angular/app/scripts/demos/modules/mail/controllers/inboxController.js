angular.module('theme.mail.inbox_controller', [])
  .controller('InboxController', ['$scope', '$location', '$theme', function($scope, $location, $theme) {
    'use strict';
    $scope.openMail = function() {
      $location.path('/read-mail');
    };

    if ($theme.get('leftbarCollapsed') == false) {
        $theme.set('leftbarCollapsed', true);

        $scope.$on('$destroy', function() {
            $theme.set('leftbarCollapsed', false);
        });
    }
  }]);