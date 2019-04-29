export interface IOrganizationData {
  name: string;
  contactInformation: string;
  type: OrganizationType;
}

export enum OrganizationType {
  Company,
  University
}
