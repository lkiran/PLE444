angular.module('theme.google_maps', [])
  .service('GMaps', ['$rootScope', '$window', function($rootScope, $window) {
    'use strict';
    this.new = function(options, instance) {
      var gmaps = new $window.GMaps(options);
      $rootScope.$broadcast('GMaps:created', {
        key: instance,
        map: gmaps
      });
    };
    this.newPanorama = function(options, instance) {
      var gmaps = $window.GMaps.createPanorama(options);
      $rootScope.$broadcast('GMaps:created', {
        key: instance,
        map: gmaps
      });
    };
  }])
  .directive('gmap', ['$timeout', 'GMaps', function($timeout, GMaps) {
    'use strict';
    return {
      restrict: 'A',
      scope: {
        options: '=',
        instance: '@'
      },
      link: function(scope, element, attr) {
        if (!attr.id) {
          attr.id = Math.random().toString(36).substring(7);
          element.attr('id', attr.id);
        }
        scope.options.el = '#' + attr.id;

        if (attr.panorama !== undefined) {
          GMaps.newPanorama(scope.options, scope.instance);
        } else {
          GMaps.new(scope.options, scope.instance);
        }
      }
    };
  }]);