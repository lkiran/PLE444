angular
  .module('theme.demos.panels', [])
  .controller('PanelsController', ['$scope', '$timeout', function($scope, $timeout) {
    'use strict';
    $scope.rootPanels = [{
      active: false
    }, {
      active: true
    }];
    $scope.alert = function(a) {
      console.log(a);
    };

    $scope.demoPanelClass = 'panel-sky';
    $scope.demoPanelHeading = 'Simple Panel Heading';
    $scope.demoTabbedPanelClass = 'panel-danger';
    $scope.demoTabbedPanelHeading = 'Tabbed Panel Heading';

    $scope.demoPanelIcon = 'fa fa-cog';

    $scope.demoDynamicIcons = function() {
      $scope.demoPanelIcon = 'fa fa-spinner fa-spin';
      $timeout(function() {
        $scope.demoPanelIcon = 'fa fa-tasks';
      }, 1500);
    };
  }]);