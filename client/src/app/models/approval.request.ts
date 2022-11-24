import { RequestStatus } from '../enums/request.status';

export interface ApprovalRequest {
  AccessRequestId: number;
  ExpiryDate: Date;
  Status: RequestStatus;
}
