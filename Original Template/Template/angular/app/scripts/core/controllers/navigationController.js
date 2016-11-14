angular
  .module('theme.core.navigation_controller', ['theme.core.services'])
  .controller('NavigationController', ['$scope', '$location', '$timeout', function($scope, $location, $timeout) {
    'use strict';
    $scope.menu = [{
      label: 'Explore',
      iconClasses: '',
      separator: true
    }, {
      label: 'Dashboard',
      iconClasses: 'ti ti-home',
      html: '<span class="badge badge-info">2</span>',
      url: '#/',
    }, {
      label: 'HTML Version',
      iconClasses: 'ti ti-cup',
      url: '../../',
    }, {
      label: 'Layouts',
      iconClasses: 'ti ti-layout',
      children: [{
        label: 'Grid Scaffolding',
        url: '#/layout-grid',
      }, {
        label: 'Horizontal Nav',
        url: '#/layout-horizontal'
      }, {
        label: 'Boxed',
        url: '#/layout-boxed'
      }]
    }, {
      label: 'UI Kit',
      iconClasses: 'ti ti-view-list-alt',
      children: [{
        label: 'Typography',
        url: '#/ui-typography'
      }, {
        label: 'Buttons',
        url: '#/ui-buttons'
      }, {
        label: 'Modals',
        url: '#/ui-modals'
      }, {
        label: 'Progress',
        url: '#/ui-progressbars'
      }, {
        label: 'Pagination',
        url: '#/ui-paginations'
      }, {
        label: 'Breadcrumbs',
        url: '#/ui-breadcrumbs'
      }, {
        label: 'Labels & Badges',
        url: '#/ui-labelsbadges',
      }, {
        label: 'Alerts',
        url: '#/ui-alerts',
      }, {
        label: 'Tabs',
        url: '#/ui-tabs',
      }, {
        label: 'FontAwesome Icons',
        url: '#/ui-icons-fontawesome',
      }, {
        label: 'Themify Icons',
        url: '#/ui-icons-themify',
      }, {
        label: 'Wells',
        url: '#/ui-wells'
      }, {
        label: 'Images & Carousel',
        url: '#/ui-imagecarousel'
      }]
    }, {
      label: 'Components',
      iconClasses: 'ti ti-control-shuffle',
      children: [{
        label: 'Tiles',
        url: '#/ui-tiles'
      }, {
        label: 'Bootbox',
        url: '#/components-bootbox'
      }, {
        label: 'Pines Notifications',
        url: '#/components-notifications'
      }, {
        label: 'Sliders & Ranges',
        url: '#/ui-sliders',
      }, {
        label: 'Pulsating Elements',
        url: '#/components-pulsate'
      }, {
        label: 'jQuery Knob',
        url: '#/components-knob'
      }]
    }, {
      label: 'Forms',
      iconClasses: 'ti ti-pencil',
      children: [{
        label: 'Form Layout',
        url: '#/form-layout',
      }, {
        label: 'Components',
        url: '#/form-components',
      }, {
        label: 'Pickers',
        url: '#/form-pickers'
      }, {
        label: 'Form Wizard',
        url: '#/form-wizard'
      }, {
        label: 'Validation',
        url: '#/form-validation',
      }, {
        label: 'Form Masks',
        url: '#/form-masks'
      }, {
        label: 'Advanced Uploaders',
        url: '#/form-fileupload',
      }, {
        label: 'WYSIWYG Editor',
        url: '#/form-wysiwyg',
      }, {
        label: 'Inline Editor',
        url: '#/form-xeditable',
      }]
    }, {
      label: 'Panels',
      iconClasses: 'ti ti-settings',
      hideOnHorizontal: true,
      children: [{
        label: 'Panels',
        url: '#/ui-panels',
      }, {
        label: 'Draggable Panels',
        url: '#/ui-advancedpanels'
      }]
    }, {
      label: 'Tables',
      iconClasses: 'ti ti-layout-grid3',
      children: [{
        label: 'Tables',
        url: '#/tables-basic'
      }, {
        label: 'ngGrid',
        url: '#/tables-data',
      }, {
        label: 'Responsive Tables',
        url: '#/tables-responsive'
      }, {
        label: 'Editable Tables',
        url: '#/tables-editable',
      }]
    }, {
      label: 'Analytics',
      iconClasses: 'ti ti-stats-up',
      hideOnHorizontal: true,
      children: [{
        label: 'Flot',
        url: '#/charts-flot',
      }, {
        label: 'Morris.js',
        url: '#/charts-morrisjs'
      }, {
        label: 'Easy Pie Chart',
        url: '#/charts-easypiechart'
      }, {
        label: 'Sparklines',
        url: '#/charts-sparklines',
      }]
    }, {
      label: 'Maps',
      iconClasses: 'ti ti-map-alt',
      hideOnHorizontal: true,
      children: [{
        label: 'Google Maps',
        url: '#/maps-google'
      }, {
        label: 'Vector Maps',
        url: '#/maps-vector',
      }]
    }, {
      label: 'Pages',
      iconClasses: 'ti ti-file',
      hideOnHorizontal: true,
      children: [{
        label: 'Profile',
        url: '#/extras-profile'
      }, {
        label: 'FAQ',
        url: '#/extras-faq',
      }, {
        label: 'Invoice',
        url: '#/extras-invoice'
      }, {
        label: 'Registration',
        url: '#/extras-registration'
      }, {
        label: 'Password Reset',
        url: '#/extras-forgotpassword'
      }, {
        label: 'Login',
        url: '#/extras-login'
      }, {
        label: '404 Page',
        url: '#/extras-404'
      }, {
        label: '500 Page',
        url: '#/extras-500'
      }]
    }, {
      label: 'Functional Apps',
      hideOnHorizontal: true,
      separator: true
    }, {
      label: 'Inbox',
      iconClasses: 'ti ti-email',
      url: '#/inbox',
      html: '<span class="badge badge-danger">3</span>'
    }, {
      label: 'Calendar',
      iconClasses: 'ti ti-calendar',
      url: '#/calendar',
      html: '<span class="badge badge-warning">1</span>'
    }];

    var setParent = function(children, parent) {
      angular.forEach(children, function(child) {
        child.parent = parent;
        if (child.children !== undefined) {
          setParent(child.children, child);
        }
      });
    };

    $scope.findItemByUrl = function(children, url) {
      for (var i = 0, length = children.length; i < length; i++) {
        if (children[i].url && children[i].url.replace('#', '') === url) {
          return children[i];
        }
        if (children[i].children !== undefined) {
          var item = $scope.findItemByUrl(children[i].children, url);
          if (item) {
            return item;
          }
        }
      }
    };

    setParent($scope.menu, null);

    $scope.openItems = []; $scope.selectedItems = []; $scope.selectedFromNavMenu = false;

    $scope.select = function(item) {
      // close open nodes
      if (item.open) {
        item.open = false;
        return;
      }
      for (var i = $scope.openItems.length - 1; i >= 0; i--) {
        $scope.openItems[i].open = false;
      }
      $scope.openItems = [];
      var parentRef = item;
      while (parentRef !== null) {
        parentRef.open = true;
        $scope.openItems.push(parentRef);
        parentRef = parentRef.parent;
      }

      // handle leaf nodes
      if (!item.children || (item.children && item.children.length < 1)) {
        $scope.selectedFromNavMenu = true;
        for (var j = $scope.selectedItems.length - 1; j >= 0; j--) {
          $scope.selectedItems[j].selected = false;
        }
        $scope.selectedItems = [];
        parentRef = item;
        while (parentRef !== null) {
          parentRef.selected = true;
          $scope.selectedItems.push(parentRef);
          parentRef = parentRef.parent;
        }
      }
    };

    $scope.highlightedItems = [];
    var highlight = function(item) {
      var parentRef = item;
      while (parentRef !== null) {
        if (parentRef.selected) {
          parentRef = null;
          continue;
        }
        parentRef.selected = true;
        $scope.highlightedItems.push(parentRef);
        parentRef = parentRef.parent;
      }
    };

    var highlightItems = function(children, query) {
      angular.forEach(children, function(child) {
        if (child.label.toLowerCase().indexOf(query) > -1) {
          highlight(child);
        }
        if (child.children !== undefined) {
          highlightItems(child.children, query);
        }
      });
    };

    // $scope.searchQuery = '';
    $scope.$watch('searchQuery', function(newVal, oldVal) {
      var currentPath = '#' + $location.path();
      if (newVal === '') {
        for (var i = $scope.highlightedItems.length - 1; i >= 0; i--) {
          if ($scope.selectedItems.indexOf($scope.highlightedItems[i]) < 0) {
            if ($scope.highlightedItems[i] && $scope.highlightedItems[i] !== currentPath) {
              $scope.highlightedItems[i].selected = false;
            }
          }
        }
        $scope.highlightedItems = [];
      } else
      if (newVal !== oldVal) {
        for (var j = $scope.highlightedItems.length - 1; j >= 0; j--) {
          if ($scope.selectedItems.indexOf($scope.highlightedItems[j]) < 0) {
            $scope.highlightedItems[j].selected = false;
          }
        }
        $scope.highlightedItems = [];
        highlightItems($scope.menu, newVal.toLowerCase());
      }
    });

    $scope.$on('$routeChangeSuccess', function() {
      if ($scope.selectedFromNavMenu === false) {
        var item = $scope.findItemByUrl($scope.menu, $location.path());
        if (item) {
          $timeout(function() {
            $scope.select(item);
          });
        }
      }
      $scope.selectedFromNavMenu = false;
      $scope.searchQuery = '';
    });
  }]);