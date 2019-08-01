"use strict";
var uComments;
(function (uComments) {
    angular.module("ucomments.services", []);
    angular.module("ucomments.controllers", []);
    var umbracoModule = angular.module("umbraco");
    umbracoModule.requires.push("ucomments.services");
    umbracoModule.requires.push("ucomments.controllers");
})(uComments || (uComments = {}));
//# sourceMappingURL=ucomments.js.map