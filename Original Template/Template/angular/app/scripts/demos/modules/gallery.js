angular
  .module('theme.demos.gallery', [
    'ui.bootstrap',
    'theme.gallery',
  ])
  .controller('GalleryController', ['$scope', '$modal', '$timeout', function($scope, $modal) {
    'use strict';
    $scope.galleryFilter = 'all';

    $scope.openImageModal = function($event) {
      $event.preventDefault();
      $event.stopPropagation();
      $modal.open({
        templateUrl: 'imageModalContent.html',
        controller: ['$scope', '$modalInstance', 'src', function($scope, $modalInstance, src) {
          $scope.src = src;
          $scope.cancel = function() {
            $modalInstance.dismiss('cancel');
          };
        }],
        size: 'lg',
        resolve: {
          src: function() {
            console.log($event.target.src.replace('thumb_', ''));
            return $event.target.src.replace('thmb_', '');
          }
        }
      });
    };
  }]);