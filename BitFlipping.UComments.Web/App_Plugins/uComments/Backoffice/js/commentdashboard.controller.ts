namespace uComments.Dashboards {
    interface IGroupsDashboardScope extends ng.IScope {
    }

    interface ICommentModel extends Services.ICommentModel {
        content: any;
    }
    interface ICommentOverlay {
        view: string;
        show: boolean;
        submit: (model: ICommentOverlay) => void;
        close: (model: ICommentOverlay) => void;
        comment: Services.ICommentModel;
    }
    interface IListViewHelper {
        clearSelection: (items, folders, selection) => void;
        deselectItem: (item, selection) => void;
        getFirstAllowedLayout: (layouts) => void;
        getLayout: (nodeId, availableLayouts) => void;
        isSelectedAll: (items, selection) => void;
        saveLayoutInLocalStorage: (nodeId, selectedLayout) => void;
        selectAllItems: (items, selection, $event) => void;
        selectHandler: (selectedItem, selectedIndex, items, selection, $event) => void;
        selectItem: (item, selection) => void;
        setLayout: (nodeID, selectedLayout, availableLayouts) => void;
        setSorting: (field, allow, options) => void;
        setSortingDirection: (col, direction, options) => void;
    }
    enum CommentStatus {
        Any,
        Pending,
        Approved,
        MarkedAsSpam,
        Trashed
    }

    class CommentDashboardController {
        static $inject: string[] = ["$scope", "$q", "filterFilter", "commentService", "listViewHelper", "assetsService", "notificationsService", "authResource", "contentResource"];

        public allowSelectAll: boolean = true;
        public sortableOptions: {};
        public loading: boolean = true;
        public comments: Array<ICommentModel> = [];
        public pagination = {
            pageNumber: 1,
            totalPages: 0,
            totalItems: 0,
            pageSize: 12
        };
        public overlay: ICommentOverlay = {
            view: "/App_Plugins/uComments/Backoffice/Views/Dashboard/commentoverlay.html",
            show: false,
            submit: (model) => {},
            close: (oldModel) => { },
            comment: null
        };

        public dashboard = {
            loading: true,
            status: CommentStatus.Any,
            searchTerm: "",
            userIsAdmin: false
        };

        constructor(private $scope: IGroupsDashboardScope,
            private $q: ng.IQService,
            private filterFilter: ng.IFilterFilter,
            private commentService: Services.ICommentService,
            private listViewHelper: IListViewHelper,
            private assetsService: umb.services.IAssetsService,
            private notificationsService: umb.services.INotificationsService,
            private authResource: umb.resources.IAuthResource,
            private contentResource: umb.resources.IContentResource) {

            assetsService.load(["/App_Plugins/uComments/Backoffice/css/comments-dashboard.css"], $scope)
                .then(() => {
                    this.init();
                }, () => {
                    this.notificationsService.error("Error", "Failed to load dependencies for Groups Management");
                });
        }

        init() {
            this.getComments();

            this.authResource.getCurrentUser().then((response) => {
                this.dashboard.userIsAdmin = (<any>response).userType === "admin";
            });
        }

        private getComments() {
            this.dashboard.loading = true;
            this.commentService.getComments(
                this.pagination.pageNumber,
                this.pagination.pageSize,
                this.dashboard.searchTerm,
                this.dashboard.status
            ).then((result) => {
                    this.comments = <Array<ICommentModel>>result.data.items;
                    var data = result.data;
                    this.pagination = {
                        pageNumber: data.pageNumber,
                        totalPages: data.totalPages,
                        totalItems: data.totalItems,
                        pageSize: data.pageSize
                    }

                    if (this.pagination.totalPages <= 1) {
                        this.pagination.pageNumber = 1;
                    }

                    this.dashboard.loading = false;

                    // Now get content for each page
                    angular.forEach(this.comments, (value, key) => {
                        this.getContent(value);
                    });
                }, (reason) => {
                    this.notificationsService.error("Error", reason);
                });
        }

        private getContent(comment: ICommentModel) {
            if (!comment) {
                return;
            }

            this.contentResource.getById(comment.pageId).then((response:any) => {
                comment.content = {
                    id: response.id,
                    name: response.name
                };
            });
        }

        public clickItem(item: Services.ICommentModel, $event) {
            this.overlay = {
                view: "/App_Plugins/uComments/Backoffice/Views/Dashboard/commentoverlay.html",
                show: true,
                submit: (model) => {
                    // TODO: Update comment data
                    this.overlay.show = false;
                },
                close: (oldModel) => {
                    this.overlay.show = false;
                },
                comment: item
            };
        }

        public selectItem(item: Services.ICommentModel, index: number, $event) {
            item.selected = !item.selected;
        }

        public selectAll($event) {
            angular.forEach(this.comments, (value, key) => {
                value.selected = !value.selected;
            });
        }

        public clearSelection() {
            for (var i = 0; i < this.comments.length; i++) {
                this.comments[i].selected = false;
            }
        }

        public isSelectedAll() {
            for (var i = 0; i < this.comments.length; i++) {
                if (!this.comments[i].selected)
                    return false;
            }
            return true;
        }

        public isAnythingSelected() {
            if (this.getSelected().length > 0)
                 return true;
            return false;
        }

        public getSelected(): ICommentModel[] {
            var selectedItems: ICommentModel[] = [];
            for (var i = 0; i < this.comments.length; i++) {
                if (this.comments[i].selected) {
                    selectedItems.push(this.comments[i]);
                }
            }
            return selectedItems;
        }

        public goToPage = (pageNumber) => {
            this.pagination.pageNumber = pageNumber;
            this.getComments();
        }

        public filter() {
            this.getComments();
        }

        public Queue<T>(items: T[], func: (item: T, commentService: Services.ICommentService) => ng.IPromise<void>) {
            var chain = this.$q.when();

            angular.forEach(items, (item, key) => {
                chain = chain.then(() => {
                    debugger;
                    return func(item, this.commentService);
                });
            });

            return chain;
        }

        public approveSelected() {
            var selected = this.getSelected();
            return this.Queue(selected, this.approve);
        }

        public approve(comment: ICommentModel, commentService: Services.ICommentService) {
            return commentService.toggleApprove(comment.id)
                .then((response) => {
                    comment.isApproved = !comment.isApproved;
                    comment.isTrashed = false;
                    comment.selected = false;
                }, (err) => {
                    this.notificationsService.error("Approve", err.reason);
                });

        }

        public trashSelected() {
            var selected = this.getSelected();
            return this.Queue(selected, this.moveToTrash);
        }

        

        public moveToTrash(comment: ICommentModel, commentService: Services.ICommentService) {
            return commentService.toggleTrash(comment.id)
                .then((response) => {
                    comment.isTrashed = !comment.isTrashed;
                    comment.isApproved = false;
                    comment.selected = false;
                }, (err) => {
                    this.notificationsService.error("Trash", err.reason);
                });
        }

        public markSpamSelected() {
            var selected = this.getSelected();
            return this.Queue(selected, this.markSpam);
        }

        public markSpam(comment: ICommentModel, commentService: Services.ICommentService) {
            return commentService.toggleSpam(comment.id)
                .then((response) => {
                    comment.isSpam = !comment.isSpam;
                    comment.selected = false;
                }, (err) => {
                    this.notificationsService.error("Mark as spam", err.reason);
                });
        }

        public getIcon(model: ICommentModel) {
            if (model.isApproved) {
                return "icon-chat-active";
            }

            if (model.isSpam) {
                return "icon-block";
            }

            if (model.isTrashed) {
                return "icon-delete";
            }

            return "icon-flag";
        }

        public replyTo(model: ICommentModel) {
            this.overlay = {
                view: "/App_Plugins/uComments/Backoffice/Views/Dashboard/commentreply.html",
                show: true,
                submit: (model) => {
                    // TODO: Update comment data
                    this.overlay.show = false;
                },
                close: (oldModel) => {
                    this.overlay.show = false;
                },
                comment: model
            };
        }
    }

    angular.module("ucomments.controllers").controller("CommentDashboardController", <any>CommentDashboardController);
}