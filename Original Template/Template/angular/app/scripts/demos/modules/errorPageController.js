angular
	.module('theme.demos.error_page', [
		'theme.core.services'
	])
	.controller('ErrorPageController', ['$scope', '$theme', function($scope, $theme) {
		'use strict';
		$theme.set('fullscreen', true);

		$scope.$on('$destroy', function() {
			$theme.set('fullscreen', false);
		});
	}]);