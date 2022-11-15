import { Component, OnDestroy, OnInit } from "@angular/core";
import { ReportConfig } from "../models/report.config";
import { AdminService, RequestConfig } from "../services/admin.service";
import { PowerBIService } from "../services/powerbi.service";

@Component({
    selector: "request-access",
    templateUrl: "./access.request.component.html"
})
export class AccessRequestComponent implements OnInit, OnDestroy {
    public workspaces!: ReportConfig[];
    public reports!: ReportConfig[];
    private requestConfigs!: RequestConfig[];

    constructor(private pbiService: PowerBIService, private adminService: AdminService) {}

    ngOnInit() {
        this.pbiService.getWorkspaces().subscribe(res => {
            this.workspaces =  res; 
        });

        this.adminService.requests.subscribe(i => {
            this.requestConfigs = i;
        });
    }

    ngOnDestroy(): void {
        this.adminService.reset();
    }

    onWorkspaceSelect(event: any) {
        this.pbiService.getReports(event.target.value).subscribe(res => this.reports = res);
    }

    requestAccess(reportId: string, reportName: string) {
        let requestConfig: RequestConfig = {
            UserId: "",
            Data : {
                ReportId: reportId,
                RequestedUser: "",
                ExpiryDate: "",
                ReportName: reportName
            }
        };
        this.adminService.addRequest(requestConfig);
    }

    isRequested(reportId: string): boolean {
        console.log(this.requestConfigs);
        return this.requestConfigs?.filter(i => i.Data.ReportId == reportId).length > 0;
    }
}