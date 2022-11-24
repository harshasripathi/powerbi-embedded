import { Component, OnInit } from '@angular/core';
import { RequestStatus } from '../enums/request.status';
import { ApprovalRequest } from '../models/approval.request';
import { RequestConfig } from '../models/request.config';
import { RequestAccessService } from '../services/request.access.service';

@Component({
  selector: 'admin',
  templateUrl: './admin.component.html',
})
export class AdminComponent implements OnInit {
  public requests: RequestConfig[] = [];

  constructor(private adminService: RequestAccessService) {}

  ngOnInit(): void {
    this.adminService.getAccessRequestsByManager('test').subscribe((i) => {
      console.log(i);
      this.requests = i;
    });
    // this.adminService.requests.subscribe((i) => {
    //   console.log(i);
    //   this.requests = i;
    // });
  }

  approveRequest(id?: number) {
    if (id) {
      let request: ApprovalRequest = {
        AccessRequestId: id,
        Status: RequestStatus.Approved,
        ExpiryDate: new Date(),
      };

      this.adminService
        .requestApproval(request)
        .subscribe((i) => console.log(i));
    }
  }

  rejectRequest(id?: number) {
    if (id) {
      let request: ApprovalRequest = {
        AccessRequestId: id,
        Status: RequestStatus.Rejected,
        ExpiryDate: new Date(),
      };

      this.adminService
        .requestApproval(request)
        .subscribe((i) => console.log(i));
    }
  }
}
