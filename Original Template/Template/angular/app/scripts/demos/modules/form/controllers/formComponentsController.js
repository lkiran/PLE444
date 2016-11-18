angular
  .module('theme.demos.forms')
  .controller('FormComponentsController', ['$scope', '$http', '$theme', function($scope, $http, $theme) {
    'use strict';
    $scope.getBrandColor = function(color) {
      return $theme.getBrandColor(color);
    };
    $scope.switchStatus = 1;
    $scope.switchStatus2 = 1;
    $scope.switchStatus3 = 1;
    $scope.switchStatus4 = 1;
    $scope.switchStatus5 = 1;
    $scope.switchStatus6 = 1;

    $scope.getLocation = function(val) {
      return $http.get('http://maps.googleapis.com/maps/api/geocode/json', {
        params: {
          address: val,
          sensor: false
        }
      }).then(function(res) {
        var addresses = [];
        angular.forEach(res.data.results, function(item) {
          addresses.push(item.formatted_address);
        });
        return addresses;
      });
    };

    $scope.colorPicked = '#fa4d4d';

    $scope.tagList = ['tag1', 'tag2'];
    $scope.select2TaggingOptions = {
      'multiple': true,
      'simple_tags': true,
      'tags': ['tag1', 'tag2', 'tag3', 'tag4'] // Can be empty list.
    };

    $scope.clear = function() {
      $scope.person.selected = undefined;
      $scope.address.selected = undefined;
      $scope.country.selected = undefined;
    };

    $scope.someGroupFn = function(item) {

      if (item.name[0] >= 'A' && item.name[0] <= 'M') {
        return 'From A - M';
      }

      if (item.name[0] >= 'N' && item.name[0] <= 'Z') {
        return 'From N - Z';
      }
    };

    // ui-select stuff
    $scope.availableColors = ['Red', 'Green', 'Blue', 'Yellow', 'Magenta', 'Maroon', 'Umbra', 'Turquoise'];

    $scope.multipleDemo = {};
    $scope.multipleDemo.colors = ['Blue', 'Red'];
    $scope.multipleDemo.colors2 = ['Blue', 'Red'];

    $scope.address = {};
    $scope.refreshAddresses = function(address) {
      var params = {
        address: address,
        sensor: false
      };
      return $http.get(
        'http://maps.googleapis.com/maps/api/geocode/json', {
          params: params
        }
      ).then(function(response) {
        $scope.addresses = response.data.results;
      });
    };

    $scope.selectedCountry = {};
    $scope.selectedCountries = {};
    $scope.countries = [];
    $http.get('assets/demo/countries.json').success(function(response) {
      $scope.countries = response;
    });
  }]);