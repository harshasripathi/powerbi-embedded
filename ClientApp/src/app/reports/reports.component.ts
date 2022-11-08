import { AfterViewInit, Component, ElementRef, Inject, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EmbedConfig } from '../models/embed.config';
import { models, service, Report } from "powerbi-client"
import { PowerBIReportEmbedComponent } from 'powerbi-client-angular';
import { LogService } from '../services/logs.service';
import { LoaderService } from '../services/loader.service';

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
    ['dataSelected', (event) => {
      this.logService.pushLog(JSON.stringify(event?.detail?.dataPoints));
    }],
    ['visualClicked', (event) => {
      if (event?.detail?.visual?.type == "card") {
        this.getVisualDataFromCard(event?.detail?.visual?.name);
      }
    }]
  ]);

  @ViewChild(PowerBIReportEmbedComponent) reportObj!: PowerBIReportEmbedComponent;


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private logService: LogService, private loader: LoaderService) {
    http.get<ReportConfig[]>(baseUrl + 'api/reports/GetReports').subscribe(result => {
      this.reports = result;
    }, error => console.error(error));
  }

  getReportConfig(event: any) {
    this.loader.setLoading(true);
    this.embedConfig.id = undefined;
    this.dataSelectLogs = [];
    let reportId = event.target.value;
    this.http.get(this.baseUrl + `api/reports/GetReportConfig/${reportId}`).subscribe((result: any) => {
      this.loader.setLoading(false);
      let report = result?.embedReports[0];
      this.embedConfig = {
        ...this.embedConfig,
        id: report?.reportId,
        embedUrl: report?.embedUrl,
        accessToken: result?.embedToken.token
      };
    }, error => console.error(error));
  }

  async getVisualDataFromCard(visualName: string) {
    const report = this.reportObj.getReport();
    const pages = await report.getPages();
    let page = pages.filter(i => i.isActive)[0];
    const visuals = await page.getVisuals();

    let visual = visuals.filter(i => i.name === visualName)[0];
    let result = await visual.exportData(models.ExportDataType.Summarized);
    this.logService.pushLog(result.data);
  }
}

interface ReportConfig {
  id: string;
  name: string;
}
