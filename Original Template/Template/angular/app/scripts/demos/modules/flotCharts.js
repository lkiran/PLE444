angular
  .module('theme.demos.flot_charts', [
    'theme.core.services',
    'theme.chart.flot'
  ])
  .controller('FlotChartsController', ['$scope', '$timeout', '$theme', function($scope, $timeout, $theme) {
    'use strict';
    var sin = [],
      cos = [];

    for (var i = 0; i < 14; i += 0.5) {
      sin.push([i, Math.sin(i)]);
      cos.push([i, Math.cos(i)]);
    }


    // Data Points

    $scope.sinusoidalData = [{
      data: sin,
      label: 'sin(x)/x'
    }, {
      data: cos,
      label: 'cos(x)'
    }];

    $scope.sinusoidalOptions = {
      series: {
        shadowSize: 0,
        lines: {
          show: true,
          lineWidth: 2
        },
        points: {
          show: true
        }
      },
      grid: {
        labelMargin: 10,
        hoverable: true,
        clickable: true,
        borderWidth: 1,
        borderColor: '#f5f5f5'
      },
      legend: {
        backgroundColor: '#fff'
      },
      yaxis: {
        min: -1.2,
        max: 1.2,
        tickColor: '#f5f5f5',
        font: {
          color: '#bdbdbd'
        }
      },
      xaxis: {
        tickColor: '#f5f5f5',
        font: {
          color: '#bdbdbd'
        }
      },
      colors: [$theme.getBrandColor('success'), $theme.getBrandColor('inverse')],
      tooltip: true,
      tooltipOpts: {
        content: 'x: %x, y: %y'
      }
    };

    $scope.sinusoidalOnClick = function() {
      // console.log(event, position, item);
    };
    $scope.sinusoidalOnHover = function() {
      // console.log(event, position, item);
    };

    // Multi Graphs in One

    var d1 = [];
    for (i = 0; i < 14; i += 0.1) {
      d1.push([i, Math.sin(i)]);
    }

    var d2 = [
      [0, 3],
      [4, 8],
      [8, 5],
      [9, 13]
    ];

    var d3 = [];
    for (i = 0; i < 15; i += 1) {
      d3.push([i, Math.cos(i) + 10]);
    }

    var d4 = [];
    for (i = 0; i < 14; i += 0.01) {
      d4.push([i, Math.sqrt(i * 10)]);
    }

    var d5 = [];
    for (i = 0; i < 15; i += 1) {
      d5.push([i, Math.sqrt(i)]);
    }

    var d6 = [];
    for (i = 0; i < 15; i += 1) {
      d6.push([i, Math.sqrt(5 * i + Math.sin(i) + 5)]);
    }

    $scope.multigraphData = [{
      data: d1,
      lines: {
        show: true,
        fill: 0.2,
        lineWidth: 1.5
      },
      shadowSize: 0
    }, {
      data: d2,
      bars: {
        show: true,
        fill: 0.2,
        lineWidth: 1.5
      },
      shadowSize: 0
    }, {
      data: d3,
      points: {
        show: true,
        fill: 0,
      },
      shadowSize: 0
    }, {
      data: d4,
      lines: {
        show: true,
        fill: 0,
        lineWidth: 1.5
      },
      shadowSize: 0
    }, {
      data: d5,
      lines: {
        show: true,
        fill: 0,
        lineWidth: 1.5
      },
      points: {
        show: true,
        fill: 0.2
      },
      shadowSize: 0
    }, {
      data: d6,
      lines: {
        show: true,
        steps: true,
        fill: 0.05,
        lineWidth: 1.5
      },
      shadowSize: 0
    }];
    $scope.multigraphOptions = {
      grid: {
        labelMargin: 10,
        hoverable: true,
        clickable: true,
        borderWidth: 1,
        borderColor: '#f5f5f5',
      },
      yaxis: {
        tickColor: '#f5f5f5',
        font: {
          color: '#bdbdbd'
        }
      },
      xaxis: {
        tickColor: '#f5f5f5',
        font: {
          color: '#bdbdbd'
        }
      },
      colors: [$theme.getBrandColor('midnightblue'), $theme.getBrandColor('danger'), $theme.getBrandColor('indigo'), $theme.getBrandColor('inverse'), $theme.getBrandColor('inverse'), $theme.getBrandColor('midnightblue')],
      tooltip: true,
      tooltipOpts: {
        content: 'x: %x, y: %y'
      }
    };

    var dxta = [],
      totalPoints = 300;
    var updateInterval = 150;

    function getRandomData() {
      if (dxta.length > 0) {
        dxta = dxta.slice(1);
      }

      while (dxta.length < totalPoints) {
        var prev = dxta.length > 0 ? dxta[dxta.length - 1] : 50,
          y = prev + Math.random() * 10 - 5;

        if (y < 0) {
          y = 0;
        } else if (y > 100) {
          y = 100;
        }

        dxta.push(y);
      }
      var res = [];
      for (i = 0; i < dxta.length; ++i) {
        res.push([i, dxta[i]]);
      }
      return res;
    }

    // Real Time Data

    $scope.realtimeData = [getRandomData()];
    $scope.realtimeOptions = {
      series: {
        lines: {
          show: true,
          lineWidth: 1.5,
          fill: 0.15,
          fillColor: {
            colors: [{
              opacity: 0.01
            }, {
              opacity: 0.3
            }]
          }
        },
        shadowSize: 0 // Drawing is faster without shadows
      },
      grid: {
        labelMargin: 10,
        hoverable: true,
        clickable: true,
        borderWidth: 1,
        borderColor: '#f5f5f5'
      },
      yaxis: {
        min: 0,
        max: 100,
        tickColor: '#f5f5f5',
        font: {
          color: '#bdbdbd'
        }
      },
      xaxis: {
        show: false
      },
      colors: ['#95a5a6'],
      tooltip: true,
      tooltipOpts: {
        content: 'CPU Utilization: %y%'
      }

    };

    var promise;
    var updateRealtimeData = function() {
      $scope.realtimeData = [getRandomData()];
      $timeout.cancel(promise);
      promise = $timeout(updateRealtimeData, updateInterval);
    };

    updateRealtimeData();

    // Stacking

    d1 = [];
    for (i = 0; i <= 10; i += 1) {
      d1.push([i, parseInt(Math.random() * 30)]);
    }
    d2 = [];
    for (i = 0; i <= 10; i += 1) {
      d2.push([i, parseInt(Math.random() * 30)]);
    }
    d3 = [];
    for (i = 0; i <= 10; i += 1) {
      d3.push([i, parseInt(Math.random() * 30)]);
    }

    $scope.stackedIsTrue = true;
    $scope.stackedBars = true;
    $scope.stackedLines = false;
    $scope.stackedSteps = false;

    var getStackedOptions = function() {
      return {
        series: {
          shadowSize: 0,
          stack: $scope.stackedIsTrue,
          lines: {
            show: $scope.stackedLines,
            fill: 0.2,
            lineWidth: 1.5,
            steps: $scope.stackedSteps,
          },
          bars: {
            show: $scope.stackedBars,
            barWidth: 0.3,
            lineWidth: 1.5
          }
        },
        grid: {
          labelMargin: 10,
          hoverable: true,
          clickable: true,
          borderWidth: 1,
          borderColor: '#f5f5f5'
        },
        yaxis: {
          tickColor: '#f5f5f5',
          font: {
            color: '#bdbdbd'
          }
        },
        xaxis: {
          tickColor: '#f5f5f5',
          font: {
            color: '#bdbdbd'
          }
        },
        colors: [$theme.getBrandColor('midnightblue'), $theme.getBrandColor('info'), $theme.getBrandColor('success')],
        tooltip: true,
        tooltipOpts: {
          content: 'X: %x | Y: %y'
        }
      };
    };

    $scope.setOption = function(type) {
      switch (type) {
        case 'Lines':
          $scope.stackedLines = true;
          $scope.stackedBars = false;
          $scope.stackedSteps = false;
          break;
        case 'Bars':
          $scope.stackedBars = true;
          $scope.stackedLines = false;
          $scope.stackedSteps = false;
          break;
        case 'Steps':
          $scope.stackedSteps = true;
          $scope.stackedBars = false;
          $scope.stackedLines = true;
          break;
        case 'Stack':
          $scope.stackedIsTrue = true;
          break;
        case 'Unstack':
          $scope.stackedIsTrue = false;
          break;
      }
      $scope.stackedOptions = getStackedOptions();
    };
    $scope.stackedOptions = getStackedOptions();
    $scope.stackedData = [d1, d2, d3];

    // Pie Chart

    $scope.pieData = [{
        label: 'Series1',
        data: 10,
        color: $theme.getBrandColor('danger')
      }, {
        label: 'Series2',
        data: 30,
        color: $theme.getBrandColor('warning')
      }, {
        label: 'Series3',
        data: 90,
        color: $theme.getBrandColor('midnightblue')
      }, {
        label: 'Series4',
        data: 70,
        color: $theme.getBrandColor('info')
      }, {
        label: 'Series5',
        data: 80,
        color: $theme.getBrandColor('success')
      }, {
        label: 'Series6',
        data: 110,
        color: $theme.getBrandColor('inverse')
      }

    ];

    $scope.pieOptions = {
      series: {
        pie: {
          show: true
        }
      },
      grid: {
        hoverable: true
      },
      tooltip: true,
      tooltipOpts: {
        content: '%p.0%, %s'
      }
    };

    $scope.donutOptions = {
      series: {
        pie: {
          innerRadius: 0.5,
          show: true
        }
      },
      grid: {
        hoverable: true
      },
      tooltip: true,
      tooltipOpts: {
        content: '%p.0%, %s'
      }
    };

    $scope.$on('$destroy', function() {
      $timeout.cancel(promise);
    });
  }]);