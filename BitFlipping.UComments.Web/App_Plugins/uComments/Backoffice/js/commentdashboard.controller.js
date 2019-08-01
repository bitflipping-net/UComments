"use strict";
var uComments;
(function (uComments) {
    var Dashboards;
    (function (Dashboards) {
        var CommentStatus;
        (function (CommentStatus) {
            CommentStatus[CommentStatus["Any"] = 0] = "Any";
            CommentStatus[CommentStatus["Pending"] = 1] = "Pending";
            CommentStatus[CommentStatus["Approved"] = 2] = "Approved";
            CommentStatus[CommentStatus["MarkedAsSpam"] = 3] = "MarkedAsSpam";
            CommentStatus[CommentStatus["Trashed"] = 4] = "Trashed";
        })(CommentStatus || (CommentStatus = {}));
        var CommentDashboardController = (function () {
            function CommentDashboardController($scope, $q, filterFilter, commentService, listViewHelper, assetsService, notificationsService, authResource, contentResource) {
                var _this = this;
                this.$scope = $scope;
                this.$q = $q;
                this.filterFilter = filterFilter;
                this.commentService = commentService;
                this.listViewHelper = listViewHelper;
                this.assetsService = assetsService;
                this.notificationsService = notificationsService;
                this.authResource = authResource;
                this.contentResource = contentResource;
                this.allowSelectAll = true;
                this.loading = true;
                this.comments = [];
                this.pagination = {
                    pageNumber: 1,
                    totalPages: 0,
                    totalItems: 0,
                    pageSize: 12
                };
                this.overlay = {
                    view: "/App_Plugins/uComments/Backoffice/Views/Dashboard/commentoverlay.html",
                    show: false,
                    submit: function (model) { },
                    close: function (oldModel) { },
                    comment: null
                };
                this.dashboard = {
                    loading: true,
                    status: CommentStatus.Any,
                    searchTerm: "",
                    userIsAdmin: false
                };
                this.goToPage = function (pageNumber) {
                    _this.pagination.pageNumber = pageNumber;
                    _this.getComments();
                };
                assetsService.load(["/App_Plugins/uComments/Backoffice/css/comments-dashboard.css"], $scope)
                    .then(function () {
                    _this.init();
                }, function () {
                    _this.notificationsService.error("Error", "Failed to load dependencies for Groups Management");
                });
            }
            CommentDashboardController.prototype.init = function () {
                var _this = this;
                this.getComments();
                this.authResource.getCurrentUser().then(function (response) {
                    _this.dashboard.userIsAdmin = response.userType === "admin";
                });
            };
            CommentDashboardController.prototype.getComments = function () {
                var _this = this;
                this.dashboard.loading = true;
                this.commentService.getComments(this.pagination.pageNumber, this.pagination.pageSize, this.dashboard.searchTerm, this.dashboard.status).then(function (result) {
                    _this.comments = result.data.items;
                    var data = result.data;
                    _this.pagination = {
                        pageNumber: data.pageNumber,
                        totalPages: data.totalPages,
                        totalItems: data.totalItems,
                        pageSize: data.pageSize
                    };
                    if (_this.pagination.totalPages <= 1) {
                        _this.pagination.pageNumber = 1;
                    }
                    _this.dashboard.loading = false;
                    angular.forEach(_this.comments, function (value, key) {
                        _this.getContent(value);
                    });
                }, function (reason) {
                    _this.notificationsService.error("Error", reason);
                });
            };
            CommentDashboardController.prototype.getContent = function (comment) {
                if (!comment) {
                    return;
                }
                this.contentResource.getById(comment.pageId).then(function (response) {
                    comment.content = {
                        id: response.id,
                        name: response.name
                    };
                });
            };
            CommentDashboardController.prototype.clickItem = function (item, $event) {
                var _this = this;
                this.overlay = {
                    view: "/App_Plugins/uComments/Backoffice/Views/Dashboard/commentoverlay.html",
                    show: true,
                    submit: function (model) {
                        _this.overlay.show = false;
                    },
                    close: function (oldModel) {
                        _this.overlay.show = false;
                    },
                    comment: item
                };
            };
            CommentDashboardController.prototype.selectItem = function (item, index, $event) {
                item.selected = !item.selected;
            };
            CommentDashboardController.prototype.selectAll = function ($event) {
                angular.forEach(this.comments, function (value, key) {
                    value.selected = !value.selected;
                });
            };
            CommentDashboardController.prototype.clearSelection = function () {
                for (var i = 0; i < this.comments.length; i++) {
                    this.comments[i].selected = false;
                }
            };
            CommentDashboardController.prototype.isSelectedAll = function () {
                for (var i = 0; i < this.comments.length; i++) {
                    if (!this.comments[i].selected)
                        return false;
                }
                return true;
            };
            CommentDashboardController.prototype.isAnythingSelected = function () {
                if (this.getSelected().length > 0)
                    return true;
                return false;
            };
            CommentDashboardController.prototype.getSelected = function () {
                var selectedItems = [];
                for (var i = 0; i < this.comments.length; i++) {
                    if (this.comments[i].selected) {
                        selectedItems.push(this.comments[i]);
                    }
                }
                return selectedItems;
            };
            CommentDashboardController.prototype.filter = function () {
                this.getComments();
            };
            CommentDashboardController.prototype.Queue = function (items, func) {
                var _this = this;
                var chain = this.$q.when();
                angular.forEach(items, function (item, key) {
                    chain = chain.then(function () {
                        debugger;
                        return func(item, _this.commentService);
                    });
                });
                return chain;
            };
            CommentDashboardController.prototype.approveSelected = function () {
                var selected = this.getSelected();
                return this.Queue(selected, this.approve);
            };
            CommentDashboardController.prototype.approve = function (comment, commentService) {
                var _this = this;
                return commentService.toggleApprove(comment.id)
                    .then(function (response) {
                    comment.isApproved = !comment.isApproved;
                    comment.isTrashed = false;
                    comment.selected = false;
                }, function (err) {
                    _this.notificationsService.error("Approve", err.reason);
                });
            };
            CommentDashboardController.prototype.trashSelected = function () {
                var selected = this.getSelected();
                return this.Queue(selected, this.moveToTrash);
            };
            CommentDashboardController.prototype.moveToTrash = function (comment, commentService) {
                var _this = this;
                return commentService.toggleTrash(comment.id)
                    .then(function (response) {
                    comment.isTrashed = !comment.isTrashed;
                    comment.isApproved = false;
                    comment.selected = false;
                }, function (err) {
                    _this.notificationsService.error("Trash", err.reason);
                });
            };
            CommentDashboardController.prototype.markSpamSelected = function () {
                var selected = this.getSelected();
                return this.Queue(selected, this.markSpam);
            };
            CommentDashboardController.prototype.markSpam = function (comment, commentService) {
                var _this = this;
                return commentService.toggleSpam(comment.id)
                    .then(function (response) {
                    comment.isSpam = !comment.isSpam;
                    comment.selected = false;
                }, function (err) {
                    _this.notificationsService.error("Mark as spam", err.reason);
                });
            };
            CommentDashboardController.prototype.getIcon = function (model) {
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
            };
            CommentDashboardController.prototype.replyTo = function (model) {
                var _this = this;
                this.overlay = {
                    view: "/App_Plugins/uComments/Backoffice/Views/Dashboard/commentreply.html",
                    show: true,
                    submit: function (model) {
                        _this.overlay.show = false;
                    },
                    close: function (oldModel) {
                        _this.overlay.show = false;
                    },
                    comment: model
                };
            };
            CommentDashboardController.$inject = ["$scope", "$q", "filterFilter", "commentService", "listViewHelper", "assetsService", "notificationsService", "authResource", "contentResource"];
            return CommentDashboardController;
        }());
        angular.module("ucomments.controllers").controller("CommentDashboardController", CommentDashboardController);
    })(Dashboards = uComments.Dashboards || (uComments.Dashboards = {}));
})(uComments || (uComments = {}));
//# sourceMappingURL=commentdashboard.controller.js.map