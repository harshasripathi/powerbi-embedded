import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ReportConfig } from "../models/report.config";
import { LoaderService } from "./loader.service";
import { LogService } from "./logs.service";

@Injectable()
export class PowerBIService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private logService: LogService, private loader: LoaderService) { }

    getReports(workspaceId: string): Observable<ReportConfig[]> {
        return this.http.get<ReportConfig[]>(this.baseUrl + `api/reports/GetReports/${workspaceId}`);
    }

    getWorkspaces() {
        return this.http.get<ReportConfig[]>(this.baseUrl + "api/reports/GetWorkspaces");
    }
}