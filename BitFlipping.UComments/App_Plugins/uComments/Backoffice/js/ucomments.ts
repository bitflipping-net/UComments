namespace uComments {
    angular.module("lc.services", []);
    angular.module("lc.controllers", []);

    var umbracoModule = angular.module("umbraco");

    umbracoModule.requires.push("lc.services");
    umbracoModule.requires.push("lc.controllers");

    export interface IGroupsDashboardModel {
        members: IGroupMemberModel[];
        groups: IGroupModel[];
    }

    export interface IMemberModel {
        id: number;
        memberId: number;
        firstName: string;
        lastName: string;
        fullName: string;
        email: string;
    }

    export interface IGroupModel {
        id: number;
        name: string;
        year: number;
        members: IGroupMemberModel[];
    }

    export interface IGroupMemberModel {
        id: number;
        groupId: number;
        memberId: number;
        sort: number;
        member: IMemberModel;
    }

    export interface IParticipantModel {
        id: number;
        nodeId: number;
        memberId: number;
        statuCode: number;
        createDate: Date;
        updateDate: Date;
        member: IMemberModel;
    }

    export enum ParticipantStatus {
        Participating,
        OnLeave,
        Cancellation,
        Absence,
        Billed
    }
}

