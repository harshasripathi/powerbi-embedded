import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ApprovalRequest } from '../models/approval.request';
import { RequestConfig } from '../models/request.config';
import { HttpService } from './http.service';

@Injectable()
export class RequestAccessService {
  private _requests = new BehaviorSubject<RequestConfig[]>([]);
  public requests = this._requests.asObservable();

  constructor(private httpService: HttpService) {}

  reset() {
    console.log('On service destroy');
  }

  addRequest(request: RequestConfig) {
    this.httpService
      .post('/admin/requestaccess', request)
      .subscribe((res) => console.log(res));
    // let requests = this._requests.getValue();
    // if (requests.filter((i) => i.ReportId == request.ReportId).length == 0) {
    //   this._requests.next(this._requests.getValue().concat(request));
    // }
  }

  removeRequest(request: RequestConfig) {
    let requests = this._requests.getValue();
    this._requests.next(requests.filter((i) => i != request));
  }

  getAccessRequestsByUser(userId: string) {
    return this.httpService.get<RequestConfig>(
      `admin/getAllRequestsByUser/${userId}`
    );
  }

  getAccessRequestsByManager(managerId: string) {
    return this.httpService.get<RequestConfig>(
      `admin/getAllRequests/${managerId}`
    );
  }

  requestApproval(approvalRequest: ApprovalRequest) {
    return this.httpService.patch('admin/approverequest', approvalRequest);
  }
}
