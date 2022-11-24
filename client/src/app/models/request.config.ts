export interface RequestConfig {
  AccessRequestId?: number;
  ManagerId: string;
  RequestedUserId: string;
  RequestedUserName: string;
  AdTenantId: string;
  WorkspaceId: string;
  ReportId: string;
  ReportName: string;
  ExpiryDate: Date;
}
