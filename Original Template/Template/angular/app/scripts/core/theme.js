angular
  .module('theme', [
    'ngCookies',
    'ngResource',
    'ngSanitize',
    'ngRoute',
    'ngAnimate',
    'easypiechart',
    'NgSwitchery',
    'sun.scrollable',
    'ui.bootstrap',
    'ui.select',
    'theme.core.templates',
    'theme.core.template_overrides',
    'theme.core.directives',
    'theme.core.main_controller',
    'theme.core.navigation_controller',
    'theme.core.messages_controller',
    'theme.core.notifications_controller',
  ])
  .constant('nanoScrollerDefaults', {
    nanoClass: 'scroll-pane',
    paneClass: 'scroll-track',
    sliderClass: 'scroll-thumb',
    contentClass: 'scroll-content'
  })
  .run(['$window', function ($window) {
    $window.ngGrid.config = {
        footerRowHeight: 40,
        headerRowHeight: 40,
        rowHeight: 40
    };
  }]);

