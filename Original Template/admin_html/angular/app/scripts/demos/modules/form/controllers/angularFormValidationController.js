angular
  .module('theme.demos.forms')
  .controller('AngularFormValidationController', ['$scope', function($scope) {
    'use strict';
    $scope.validateDemoForm = {};
    $scope.form = {};

    $scope.canResetValidationForm = function() {
      return $scope.form.validateDemoForm.$dirty;
    };

    $scope.resetValidationForm = function() {
      $scope.validateDemoForm.required = '';
      $scope.validateDemoForm.minlength = '';
      $scope.validateDemoForm.maxlength = '';
      $scope.validateDemoForm.rangelength = '';
      $scope.validateDemoForm.pattern = '';
      $scope.validateDemoForm.email = '';
      $scope.validateDemoForm.url = '';
      $scope.validateDemoForm.digits = '';
      $scope.validateDemoForm.digits_min = '';
      $scope.validateDemoForm.digits_max = '';
      $scope.validateDemoForm.digits_minmax = '';
      $scope.validateDemoForm.alphanumeric = '';
      $scope.validateDemoForm.password = '';
      $scope.validateDemoForm.confirm_password = '';
      $scope.validateDemoForm.terms = '';
      $scope.form.validateDemoForm.$setPristine();
    };

    $scope.canSubmitValidationForm = function() {
      return $scope.form.validateDemoForm.$valid;
    };

    $scope.submit = function() {

    };
  }]);