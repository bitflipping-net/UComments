// <reference path="anuglarjs"/>
// <reference path="jquery"/>

module uComments.Services {
    export interface IPagedModel<T> {
        items: T[];
        pageNumber: number;
        pageSize: number;
        totalItems: number;
        totalPages: number;
    }

    export interface ICommentModel {
        id: number;
        pageId: number;
        author: string;
        bodyText: string;
        isApproved: boolean;
        isTrashed: boolean;
        isSpam: boolean;
        parentId: boolean;
        selected?: boolean;
    }

    export interface IPagedCommentFilter {
        pageId: number;
        term: string;
    }

    export interface ICommentService {
        getComments: (page: number, pageSize: number, searchTerm: string, status: number) => ng.IHttpPromise<IPagedModel<ICommentModel>>;
        getComment: (commentId: number) => ng.IHttpPromise<ICommentModel>;
        updateComment: (commentId: number, comment: ICommentModel) => ng.IHttpPromise<{}>;
        toggleSpam: (commentId: number) => ng.IHttpPromise<{}>;
        toggleApprove: (commentId: number) => ng.IHttpPromise<{}>;
        toggleTrash: (commentId: number) => ng.IHttpPromise<{}>;
        createComment: (comment: ICommentModel) => ng.IHttpPromise<{}>;
        deleteComment: (commentId: number) => ng.IHttpPromise<{}>;
    }

    export class CommentService implements ICommentService {
        static $inject: string[] = ["$http"];
        private baseUrl = "/umbraco/backoffice/uComments/CommentDashboard";

        constructor(private $http: ng.IHttpService) {
        }

        public getComments(page: number, pageSize: number, searchTerm: string, status: number) {
            var query = $.param({
                pageNumber: page,
                pageSize: pageSize,
                searchTerm: searchTerm,
                status: status
            });
            return this.$http.get<IPagedModel<ICommentModel>>(this.baseUrl + "/getComments?" + query);
        }

        public getComment(id: number) {
            return this.$http.get<ICommentModel>(this.baseUrl + "/getComment?id=" + id);
        }

        public updateComment(commentId:number, comment: ICommentModel) {
            return this.$http.patch(this.baseUrl + "/updateComment/" + commentId, comment);
        }

        public toggleSpam(commentId: number) {
            return this.$http.patch(this.baseUrl + "/toggleSpam/" + commentId, null);
        }

        public toggleApprove(commentId: number) {
            return this.$http.patch(this.baseUrl + "/toggleApprove/" + commentId, null);
        }

        public toggleTrash(commentId: number) {
            return this.$http.patch(this.baseUrl + "/toggleTrash/" + commentId, null);
        }
        public createComment(comment: ICommentModel) {
            return this.$http.post<ICommentModel>(this.baseUrl + "/createComment/", comment);
        }

        public deleteComment(commentId: number) {
            return this.$http.delete(this.baseUrl + "/deleteComment/" + commentId);
        }
    }

    angular.module("ucomments.services").service("commentService", CommentService);
}