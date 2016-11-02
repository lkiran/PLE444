angular.module('theme.demos.vector_maps', [
    'theme.vector_maps'
  ])
  .config(['$routeProvider', function($routeProvider) {
    'use strict';
    $routeProvider
      .when('/maps-vector', {
        templateUrl: 'views/maps-vector.html',
        resolve: {
          loadJqvmap: ['$ocLazyLoad', function($ocLazyLoad) {
            return $ocLazyLoad.load([
              'assets/plugins/jvectormap/jquery-jvectormap-world-mill-en.js',
              'assets/plugins/jvectormap/jquery-jvectormap-cn-mill-en.js',
              'assets/plugins/jvectormap/jquery-jvectormap-dk-mill-en.js',
              'assets/plugins/jvectormap/jquery-jvectormap-europe-mill-en.js',
              'assets/plugins/jvectormap/jquery-jvectormap-in-mill-en.js',
              'assets/plugins/jvectormap/jquery-jvectormap-nl-mill-en.js',
              'assets/plugins/jvectormap/jquery-jvectormap-se-mill-en.js',
              'assets/plugins/jvectormap/jquery-jvectormap-us-aea-en.js',
              'assets/plugins/jvectormap/jquery-jvectormap-us-ny-newyork-mill-en.js',
            ]);
          }]
        }
      });
  }])
  .controller('VectorMapsController', ['$scope', '$window', function($scope, $window) {
    'use strict;'
    $scope.worldmap = {
      map: 'world_mill_en',
      backgroundColor: 'transparent',
        regionStyle: {
          initial: {
            fill: '#8d8d8d'
        }
      }
    };
    $scope.china = {
      map: 'cn_mill_en',
      backgroundColor: 'transparent',
        regionStyle: {
          initial: {
            fill: '#8d8d8d'
        }
      }
    };
    $scope.denmark = {
      map: 'dk_mill_en',
      backgroundColor: 'transparent',
        regionStyle: {
          initial: {
            fill: '#8d8d8d'
        }
      }
    };
    $scope.europe = {
      map: 'europe_mill_en'   ,
      backgroundColor: 'transparent',
        regionStyle: {
          initial: {
            fill: '#8d8d8d'
        }
      }
    };
    $scope.india = {
      map: 'in_mill_en',
      backgroundColor: 'transparent',
        regionStyle: {
          initial: {
            fill: '#8d8d8d'
        }
      }
    };
    $scope.netherlands = {
      map: 'nl_mill_en',
      backgroundColor: 'transparent',
        regionStyle: {
          initial: {
            fill: '#8d8d8d'
        }
      }
    };
    $scope.sweden = {
      map: 'se_mill_en',
      backgroundColor: 'transparent',
        regionStyle: {
          initial: {
            fill: '#8d8d8d'
        }
      }
    };
    $scope.usa = {
      map: 'us_aea_en',
      backgroundColor: 'transparent',
        regionStyle: {
          initial: {
            fill: '#8d8d8d'
        }
      }
    };
    $scope.usaNy = {
      map: 'us-ny-newyork_mill_en',
      backgroundColor: 'transparent',
        regionStyle: {
          initial: {
            fill: '#8d8d8d'
        }
      }
    };
  }]);