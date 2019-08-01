module uComments.Services {
    export interface ICommentModel {
        id: number;
        fullName: string;
        message: string;
        isApproved: boolean;
        isTrashed: boolean;
    }

    export interface ICommentService {
        getComments: (page: number, pageSize: number, filter: {}) => ng.IHttpPromise<Array<ICommentModel>>;
        updateComment: (id: number, comment: ICommentModel) => ng.IHttpPromise<{}>;
        deleteComment: (id: number) => ng.IHttpPromise<{}>;
    }

    export class CommentService implements ICommentService {
        static $inject: string[] = ["$http"];
        private baseUrl = "/umbraco/backoffice/uComments/Comment";

        constructor(private $http: ng.IHttpService) {

        }

        public getComments(page: number, pageSize: number, filter: {}) {
            return this.$http.get<Array<ICommentModel>>(this.baseUrl + "/getComments?page=" + page + "&pageSize=" + pageSize + "&filter=" + encodeURI(JSON.stringify(filter)));
        }

        public updateComment(commentId:number, comment: ICommentModel) {
            return this.$http.post(this.baseUrl + "/updateComment/" + commentId, comment);
        }

        public deleteComment(commentId: number) {
            return this.$http.delete(this.baseUrl + "/deleteComment/" + commentId);
        }

        public createComment(comment: ICommentModel) {
            return this.$http.post(this.baseUrl + "/createComment/", comment);
        }
    }

    angular.module("ucomments.services").service("commentService", CommentService);
}