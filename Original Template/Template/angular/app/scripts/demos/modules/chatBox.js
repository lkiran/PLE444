angular
  .module('theme.demos.chatbox', [])
  .controller('ChatRoomController', ['$scope', '$timeout', '$window', function($scope, $t, $window) {
    'use strict';
    var avatars = ['potter.png', 'tennant.png', 'johansson.png', 'jackson.png', 'jobs.png'];
    $scope.messages = [];
    $scope.userText = '';
    $scope.userTyping = false;
    $scope.userAvatar = 'johansson.png';
    $scope.cannedResponses = [
      'Go on...',
      'Why, thank you!',
      'I will let you know.'
    ];

    $scope.sendMessage = function(msg) {
      var im = {
        class: 'me',
        avatar: 'jackson.png',
        text: msg
      };
      this.messages.push(im);
      this.userText = '';

      $t(function() {
        $scope.userAvatar = $window._.shuffle(avatars).shift();
        $scope.userTyping = true;
      }, 500);

      $t(function() {
        var reply = $window._.shuffle($scope.cannedResponses).shift();
        var im = {
          class: 'chat-success',
          avatar: $scope.userAvatar,
          text: reply
        };
        $scope.userTyping = false;
        $scope.messages.push(im);
      }, 1200);
    };
  }]);