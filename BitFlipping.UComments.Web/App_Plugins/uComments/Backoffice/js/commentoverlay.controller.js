"use strict";
var uComments;
(function (uComments) {
    var Dashboards;
    (function (Dashboards) {
        var CommentOverlayController = (function () {
            function CommentOverlayController($scope, commentService) {
                this.$scope = $scope;
                this.commentService = commentService;
                console.log($scope);
            }
            CommentOverlayController.$inject = ["$scope", "commentService"];
            return CommentOverlayController;
        }());
        angular.module("ucomments.controllers").controller("CommentOverlayController", CommentOverlayController);
    })(Dashboards = uComments.Dashboards || (uComments.Dashboards = {}));
})(uComments || (uComments = {}));
//# sourceMappingURL=commentoverlay.controller.js.map