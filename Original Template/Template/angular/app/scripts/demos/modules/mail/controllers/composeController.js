angular.module('theme.mail.compose_controller', ['textAngular'])
  .controller('ComposeController', ['$scope', '$theme', function($scope, $theme) {
    'use strict';
    $scope.mailBody = null;

    if ($theme.get('leftbarCollapsed') == false) {
        $theme.set('leftbarCollapsed', true);

        $scope.$on('$destroy', function() {
            $theme.set('leftbarCollapsed', false);
        });
    }
  }]);