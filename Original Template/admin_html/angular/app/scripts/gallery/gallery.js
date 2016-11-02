angular
  .module('theme.gallery', [])
  .directive('gallery', function() {
    'use strict';
    return {
      restrict: 'A',
      scope: {
        filterClass: '@filterClass',
        sortClass: '@sortClass'
      },
      link: function(scope, element) {
        element.shuffle({
          itemSelector: '.item'
        });

        angular.element('.' + scope.filterClass).click(function(e) {
          e.preventDefault();
          angular.element('.' + scope.filterClass).removeClass('active');
          angular.element(this).addClass('active');
          var groupName = angular.element(this).attr('data-group');
          element.shuffle('shuffle', groupName);
        });
        angular.element('.' + scope.sortClass).click(function(e) {
          e.preventDefault();
          var opts = {
            reverse: angular.element(this).data('order') === 'desc',
            by: function(el) {
              return el.data(el.data('data-sort'));
            }
          };
          angular.element('.' + scope.sortClass).removeClass('active');
          angular.element(this).addClass('active');
          element.shuffle('sort', opts);
        });
      }
    };
  });