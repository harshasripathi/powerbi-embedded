import {
  AfterViewInit,
  Component,
  ElementRef,
  Inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EmbedConfig } from '../models/embed.config';
import { models, service, Report } from 'powerbi-client';
import { PowerBIReportEmbedComponent } from 'powerbi-client-angular';
import { LogService } from '../services/logs.service';
import { LoaderService } from '../services/loader.service';
import { ReportConfig } from '../models/report.config';
import { PowerBIService } from '../services/powerbi.service';

@Component({
  selector: 'reports',
  templateUrl: './reports.component.html',
})
export class ReportsComponent implements OnInit {
  private readonly workspaceId: string = 'd1da2ed9-25c5-4c94-af56-ed71d65b4945';
  //'db097125-a738-4629-9a9b-93e6367f921e';
  //'d1da2ed9-25c5-4c94-af56-ed71d65b4945';
  //"839940d7-ba79-4b1c-bf12-2f48e476acec";
  // Servian //"db097125-a738-4629-9a9b-93e6367f921e";
  public reports: ReportConfig[] = [];
  public phasedEmbedding: boolean = false;
  public reportClass: string = 'report-container';
  public embedConfig: EmbedConfig = {
    type: 'report',
    id: undefined,
    embedUrl: undefined,
    accessToken: undefined,
    tokenType: models.TokenType.Embed,
  };
  public dataSelectLogs: string[] = [];
  public eventHandlersMap = new Map<
    string,
    (event?: service.ICustomEvent<any>) => void
  >([
    [
      'dataSelected',
      (event) => {
        this.logService.pushLog(JSON.stringify(event?.detail?.dataPoints));
      },
    ],
    [
      'visualClicked',
      (event) => {
        if (event?.detail?.visual?.type == 'card') {
          this.getVisualDataFromCard(event?.detail?.visual?.name);
        }
      },
    ],
  ]);

  @ViewChild(PowerBIReportEmbedComponent)
  reportObj!: PowerBIReportEmbedComponent;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private logService: LogService,
    private loader: LoaderService,
    private pbiService: PowerBIService
  ) {}

  ngOnInit() {
    this.pbiService.getReports(this.workspaceId).subscribe(
      (result) => {
        console.log(result);
        this.reports = result;
      },
      (error) => console.log(error)
    );
  }
  getReportConfig(event: any) {
    this.loader.setLoading(true);
    this.embedConfig.id = undefined;
    this.dataSelectLogs = [];
    let reportId = event.target.value;
    this.http
      .get(
        this.baseUrl +
          `api/reports/GetReportConfig/${this.workspaceId}/${reportId}`
      )
      .subscribe(
        (result: any) => {
          this.loader.setLoading(false);
          console.log(result);
          let report = result?.EmbedReports[0];
          this.embedConfig = {
            ...this.embedConfig,
            id: report?.ReportId,
            embedUrl: report?.EmbedUrl,
            accessToken: result?.EmbedToken.token,
          };
        },
        (error) => console.error(error)
      );
  }

  async getVisualDataFromCard(visualName: string) {
    const report = this.reportObj.getReport();
    const pages = await report.getPages();
    let page = pages.filter((i) => i.isActive)[0];
    const visuals = await page.getVisuals();

    let visual = visuals.filter((i) => i.name === visualName)[0];
    let result = await visual.exportData(models.ExportDataType.Summarized);
    this.logService.pushLog(result.data);
  }
}
