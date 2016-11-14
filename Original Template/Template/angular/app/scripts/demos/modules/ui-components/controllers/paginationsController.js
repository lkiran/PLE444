angular
  .module('theme.demos.ui_components')
  .controller('PaginationAndPagingController', ['$scope', '$window', function($scope, $window) {
    'use strict';
    $scope.totalItems = 64;
    $scope.currentPage = 4;

    $scope.pageChanged = function() {
      console.log('Page changed to: ' + $scope.currentPage);
    };

    $scope.maxSize = 5;
    $scope.bigTotalItems = 175;
    $scope.bigCurrentPage = 1;

    $scope.dpWithCallback = {
      onSelectedDateChanged: function(event, date) {
        $window.alert('Selected date: ' + $window.moment(date).format('Do, MMM YYYY'));
      }
    };
  }]);