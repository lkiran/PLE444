angular
  .module('theme.demos.morris_charts', [
    'theme.core.services',
    'theme.chart.morris'
  ])
  .config(['$routeProvider', function($routeProvider) {
    'use strict';
    $routeProvider
      .when('/charts-morrisjs', {
        templateUrl: 'views/charts-morrisjs.html',
        resolve: {
          loadMorris: ['$ocLazyLoad', function($ocLazyLoad) {
            return $ocLazyLoad.load([
              'bower_components/raphael/raphael.js',
              'bower_components/morris.js/morris.js'
            ]);
          }]
        }
      });
  }])
  .controller('SvgChartsController', ['$scope', '$theme', function($scope, $theme) {
    'use strict';
    $scope.lineChart = {
      data: [{
        y: '2006',
        a: 100,
        b: 90
      }, {
        y: '2007',
        a: 75,
        b: 65
      }, {
        y: '2008',
        a: 50,
        b: 40
      }, {
        y: '2009',
        a: 75,
        b: 65
      }, {
        y: '2010',
        a: 50,
        b: 40
      }, {
        y: '2011',
        a: 75,
        b: 65
      }, {
        y: '2012',
        a: 100,
        b: 90
      }],
      xkey: 'y',
      ykeys: ['a', 'b'],
      labels: ['Series A', 'Series B'],
      lineColors: [$theme.getBrandColor('inverse'), $theme.getBrandColor('midnightblue')]
    };
    $scope.barChart = {
      data: [{
        y: '2006',
        a: 100,
        b: 90
      }, {
        y: '2007',
        a: 75,
        b: 65
      }, {
        y: '2008',
        a: 50,
        b: 40
      }, {
        y: '2009',
        a: 75,
        b: 65
      }, {
        y: '2010',
        a: 50,
        b: 40
      }, {
        y: '2011',
        a: 75,
        b: 65
      }, {
        y: '2012',
        a: 100,
        b: 90
      }],
      xkey: 'y',
      ykeys: ['a', 'b'],
      labels: ['Series A', 'Series B'],
      barColors: [$theme.getBrandColor('inverse'), $theme.getBrandColor('midnightblue')]
    };
    $scope.donutChart = {
      data: [{
        label: 'Download Sales',
        value: 12
      }, {
        label: 'In-Store Sales',
        value: 30
      }, {
        label: 'Mail-Order Sales',
        value: 20
      }],
      colors: [$theme.getBrandColor('grape'), $theme.getBrandColor('inverse'), $theme.getBrandColor('green')]
    };
    $scope.areaChart = {
      data: [{
        y: '2006',
        a: 100,
        b: 90
      }, {
        y: '2007',
        a: 75,
        b: 65
      }, {
        y: '2008',
        a: 50,
        b: 40
      }, {
        y: '2009',
        a: 75,
        b: 65
      }, {
        y: '2010',
        a: 50,
        b: 40
      }, {
        y: '2011',
        a: 75,
        b: 65
      }, {
        y: '2012',
        a: 100,
        b: 90
      }],
      xkey: 'y',
      ykeys: ['a', 'b'],
      labels: ['Series A', 'Series B'],
      lineColors: [$theme.getBrandColor('midnightblue'), $theme.getBrandColor('inverse')]
    };
  }]);