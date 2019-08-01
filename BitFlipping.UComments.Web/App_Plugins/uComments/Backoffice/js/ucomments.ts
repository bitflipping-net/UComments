namespace uComments {
    angular.module("ucomments.services", []);
    angular.module("ucomments.controllers", []);

    var umbracoModule = angular.module("umbraco");

    umbracoModule.requires.push("ucomments.services");
    umbracoModule.requires.push("ucomments.controllers");
}

