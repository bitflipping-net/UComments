"use strict";
var uComments;
(function (uComments) {
    var Services;
    (function (Services) {
        var CommentService = (function () {
            function CommentService($http) {
                this.$http = $http;
                this.baseUrl = "/umbraco/backoffice/uComments/CommentDashboard";
            }
            CommentService.prototype.getComments = function (page, pageSize, searchTerm, status) {
                var query = $.param({
                    pageNumber: page,
                    pageSize: pageSize,
                    searchTerm: searchTerm,
                    status: status
                });
                return this.$http.get(this.baseUrl + "/getComments?" + query);
            };
            CommentService.prototype.getComment = function (id) {
                return this.$http.get(this.baseUrl + "/getComment?id=" + id);
            };
            CommentService.prototype.updateComment = function (commentId, comment) {
                return this.$http.patch(this.baseUrl + "/updateComment/" + commentId, comment);
            };
            CommentService.prototype.toggleSpam = function (commentId) {
                return this.$http.patch(this.baseUrl + "/toggleSpam/" + commentId, null);
            };
            CommentService.prototype.toggleApprove = function (commentId) {
                return this.$http.patch(this.baseUrl + "/toggleApprove/" + commentId, null);
            };
            CommentService.prototype.toggleTrash = function (commentId) {
                return this.$http.patch(this.baseUrl + "/toggleTrash/" + commentId, null);
            };
            CommentService.prototype.createComment = function (comment) {
                return this.$http.post(this.baseUrl + "/createComment/", comment);
            };
            CommentService.prototype.deleteComment = function (commentId) {
                return this.$http.delete(this.baseUrl + "/deleteComment/" + commentId);
            };
            CommentService.$inject = ["$http"];
            return CommentService;
        }());
        Services.CommentService = CommentService;
        angular.module("ucomments.services").service("commentService", CommentService);
    })(Services = uComments.Services || (uComments.Services = {}));
})(uComments || (uComments = {}));
//# sourceMappingURL=comment.service.js.map