import { Component, OnDestroy, OnInit } from '@angular/core';
import { ReportConfig } from '../models/report.config';
import { RequestAccessService } from '../services/request.access.service';
import { PowerBIService } from '../services/powerbi.service';
import { AuthService } from '../services/auth.service';
import { environment } from 'src/environments/environment';
import { RequestConfig } from '../models/request.config';

@Component({
  selector: 'request-access',
  templateUrl: './access.request.component.html',
})
export class AccessRequestComponent implements OnInit, OnDestroy {
  public workspaces!: ReportConfig[];
  public reports!: ReportConfig[];
  private workspaceId!: string;
  private requestConfigs!: RequestConfig[];
  private requestedAccess!: RequestConfig[];

  constructor(
    private pbiService: PowerBIService,
    private raService: RequestAccessService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    let user = this.authService.user;
    this.pbiService.getWorkspaces().subscribe((res) => {
      this.workspaces = res;
    });

    this.raService
      .getAccessRequestsByUser(user.idTokenClaims?.oid!)
      .subscribe((res) => (this.requestedAccess = res));

    this.raService.requests.subscribe((i) => {
      this.requestConfigs = i;
    });
  }

  ngOnDestroy(): void {
    this.raService.reset();
  }

  onWorkspaceSelect(event: any) {
    this.workspaceId = event.target.value;
    this.pbiService
      .getReports(this.workspaceId)
      .subscribe((res) => (this.reports = res));
  }

  requestAccess(reportId: string, reportName: string) {
    let user = this.authService.user;
    let expiryDate = new Date();
    expiryDate.setDate(
      expiryDate.getDate() + environment.defaultAccessRequestExpiry
    );
    let requestConfig: RequestConfig = {
      ManagerId: '',
      ReportId: reportId,
      RequestedUserId: user.idTokenClaims!.oid!,
      RequestedUserName: user.username,
      AdTenantId: user.tenantId,
      WorkspaceId: this.workspaceId,
      ExpiryDate: expiryDate,
      ReportName: reportName,
    };
    this.raService.addRequest(requestConfig);
  }

  isRequested(reportId: string): boolean {
    console.log(reportId);
    console.log(this.requestedAccess);
    return (
      this.requestedAccess?.filter((i) => i.ReportId == reportId).length > 0
    );
    // return (
    //   this.requestConfigs?.filter((i) => i.ReportId == reportId).length > 0
    // );
  }
}
