namespace uComments.Dashboards {

    class CommentOverlayController {
        static $inject: string[] = ["$scope", "commentService"];

        constructor(
            private $scope: ng.IScope,
            private commentService: Services.ICommentService) {
            console.log($scope);
        }

    }
    
    angular.module("ucomments.controllers").controller("CommentOverlayController", <any>CommentOverlayController);

}