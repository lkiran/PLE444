angular
  .module('theme.demos.forms')
  .controller('ImageCropController', ['$scope', function($scope) {
    'use strict';
    $scope.cropped = false;
    var imgBounds;
    $scope.setBounds = function(bounds) {
      imgBounds = bounds;
    };
    $scope.selected = function(coords) {
      $scope.imageWidth = imgBounds[0];
      $scope.containerWidth = coords.w;
      $scope.containerHeight = coords.h;
      $scope.imageTop = -coords.y;
      $scope.imageLeft = -coords.x;
      $scope.cropped = true;
    };
  }]);