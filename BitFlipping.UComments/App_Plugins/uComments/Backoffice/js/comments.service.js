var uComments;
(function (uComments) {
    var Services;
    (function (Services) {
        var CommentService = /** @class */ (function () {
            function CommentService($http) {
                this.$http = $http;
                this.baseUrl = "/umbraco/backoffice/uComments/Comment";
            }
            CommentService.prototype.getComments = function (page, pageSize, filter) {
                return this.$http.get(this.baseUrl + "/getComments?page=" + page + "&pageSize=" + pageSize + "&filter=" + encodeURI(JSON.stringify(filter)));
            };
            CommentService.prototype.updateComment = function (commentId, comment) {
                return this.$http.post(this.baseUrl + "/updateComment/" + commentId, comment);
            };
            CommentService.prototype.deleteComment = function (commentId) {
                return this.$http.delete(this.baseUrl + "/deleteComment/" + commentId);
            };
            CommentService.prototype.createComment = function (comment) {
                return this.$http.post(this.baseUrl + "/createComment/", comment);
            };
            CommentService.$inject = ["$http"];
            return CommentService;
        }());
        Services.CommentService = CommentService;
        angular.module("ucomments.services").service("commentService", CommentService);
    })(Services = uComments.Services || (uComments.Services = {}));
})(uComments || (uComments = {}));
//# sourceMappingURL=comments.service.js.map