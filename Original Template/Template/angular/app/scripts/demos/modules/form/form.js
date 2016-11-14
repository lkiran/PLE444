angular
  .module('theme.demos.forms', [
    'flow',
    'xeditable',
    'theme.core.directives',
    'theme.core.services'
  ])
  .config(['flowFactoryProvider', '$routeProvider', function(flowFactoryProvider, $routeProvider) {
    'use strict';
    flowFactoryProvider.defaults = {
      target: '',
      permanentErrors: [500, 501],
      maxChunkRetries: 1,
      chunkRetryInterval: 5000,
      simultaneousUploads: 1
    };
    flowFactoryProvider.on('catchAll', function(event) {
      console.log('catchAll', event);
    });

    $routeProvider
      .when('/form-imagecrop', {
        templateUrl: 'views/form-imagecrop.html',
        resolve: {
          loadJcrop: ['$ocLazyLoad', function($ocLazyLoad) {
            return $ocLazyLoad.load([
              'assets/plugins/jcrop/js/jquery.Jcrop.min.js'
            ]);
          }]
        }
      })
      .when('/form-wizard', {
        templateUrl: 'views/form-wizard.html',
        resolve: {
          loadStepy: ['$ocLazyLoad', function($ocLazyLoad) {
            return $ocLazyLoad.load([
              'bower_components/jquery-validation/dist/jquery.validate.js',
              'bower_components/stepy/lib/jquery.stepy.js'
            ]);
          }]
        }
      })
      .when('/form-masks', {
        templateUrl: 'views/form-masks.html',
        resolve: {
          loadMasks: ['$ocLazyLoad', function($ocLazyLoad) {
            return $ocLazyLoad.load([
              'bower_components/jquery.inputmask/dist/jquery.inputmask.bundle.js'
            ]);
          }]
        }
      });
  }]);