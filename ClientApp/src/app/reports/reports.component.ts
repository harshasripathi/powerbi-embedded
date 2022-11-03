import { AfterViewInit, Component, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EmbedConfig } from '../models/embed.config';
import { models, service, Report } from "powerbi-client"
import { PowerBIReportEmbedComponent } from 'powerbi-client-angular';

@Component({
  selector: 'reports',
  templateUrl: './reports.component.html'
})
export class ReportsComponent {
  public reports: ReportConfig[] = [];
  public phasedEmbedding: boolean = false;
  public reportClass: string = "report-container";
  public embedConfig: EmbedConfig = {
    type: "report",
    id: undefined,
    embedUrl: undefined,
    accessToken: undefined,
    tokenType: models.TokenType.Embed
  };
  public dataSelectLogs: string[] = [];
  public eventHandlersMap = new Map<string, (event?: service.ICustomEvent<any>) => void>([
    ['dataSelected', (event) => this.dataSelectLogs.push(JSON.stringify(event?.detail?.dataPoints))]
  ]);

  @ViewChild(PowerBIReportEmbedComponent) reportObj!: PowerBIReportEmbedComponent;


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    http.get<ReportConfig[]>(baseUrl + 'api/reports/GetReports').subscribe(result => {
      this.reports = result;
    }, error => console.error(error));
  }

  getReportConfig(event: any) {
    this.embedConfig.id = undefined;
    this.dataSelectLogs = [];
    let reportId = event.target.value;
    this.http.get(this.baseUrl + `api/reports/GetReportConfig/${reportId}`).subscribe((result: any) => {
      let report = result?.embedReports[0];
      this.embedConfig = {
        ...this.embedConfig,
        id: report?.reportId,
        embedUrl: report?.embedUrl,
        accessToken: result?.embedToken.token
      };
    }, error => console.error(error));
  }
}

interface ReportConfig {
  id: string;
  name: string;
}
