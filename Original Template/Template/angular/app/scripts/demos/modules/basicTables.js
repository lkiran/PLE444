angular
	.module('theme.demos.basic_tables', [])
	.controller('TablesBasicController', ['$scope', function($scope) {
		'use strict';
		$scope.data = {
			headings: ['#', 'First Name', 'Last Name', 'Username'],
			rows: [
				['1', 'Mark', 'Otto', '@mdo'],
				['2', 'Jacob', 'Thornton', '@fat'],
				['3', 'Larry', 'the Bird', '@twitter']
			]
		};
	}]);