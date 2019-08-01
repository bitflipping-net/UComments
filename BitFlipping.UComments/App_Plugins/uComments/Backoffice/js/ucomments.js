var uComments;
(function (uComments) {
    angular.module("lc.services", []);
    angular.module("lc.controllers", []);
    var umbracoModule = angular.module("umbraco");
    umbracoModule.requires.push("lc.services");
    umbracoModule.requires.push("lc.controllers");
    var ParticipantStatus;
    (function (ParticipantStatus) {
        ParticipantStatus[ParticipantStatus["Participating"] = 0] = "Participating";
        ParticipantStatus[ParticipantStatus["OnLeave"] = 1] = "OnLeave";
        ParticipantStatus[ParticipantStatus["Cancellation"] = 2] = "Cancellation";
        ParticipantStatus[ParticipantStatus["Absence"] = 3] = "Absence";
        ParticipantStatus[ParticipantStatus["Billed"] = 4] = "Billed";
    })(ParticipantStatus = uComments.ParticipantStatus || (uComments.ParticipantStatus = {}));
})(uComments || (uComments = {}));
//# sourceMappingURL=ucomments.js.map