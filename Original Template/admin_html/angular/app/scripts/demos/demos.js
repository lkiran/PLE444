angular
  .module('theme.demos', [
    'oc.lazyLoad',
    'theme.demos.calendar',
    'theme.demos.canvas_charts',
    'theme.demos.nvd3_charts',
    'theme.demos.flot_charts',
    'theme.demos.morris_charts',
    'theme.demos.sparkline_charts',
    'theme.demos.ui_components',
    'theme.demos.basic_tables',
    'theme.demos.boxed_layout',
    'theme.demos.horizontal_layout',
    'theme.demos.dashboard',
    'theme.demos.chatbox',
    'theme.demos.gallery',
    'theme.demos.editable_table',
    'theme.demos.google_maps',
    'theme.demos.vector_maps',
    'theme.demos.ng_grid',
    'theme.demos.signup_page',
    'theme.demos.not_found',
    'theme.demos.error_page',
    'theme.demos.tasks',
    'theme.demos.mail',
  ])
  .directive('img', ['$timeout', function ($t) {
      // NOTE: this affects all <img> tags
      // Remove this directive for production
    'use strict';
      return {
      restrict: 'E',
      link: function (scope, element) {
        $t ( function () {
            var src = element.attr('src') || element.attr('ng-src');
          if (src.match(/assets\/demo/)) {
            element.attr('src', 'http://placehold.it/400&text=Placeholder');
          }
        }, 10);
      }
      };
  }]);