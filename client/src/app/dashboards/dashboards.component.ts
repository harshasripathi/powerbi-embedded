import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { models } from "powerbi-client"
import { TokenType } from '@angular/compiler';
import { EmbedConfig } from '../models/embed.config';

@Component({
  selector: 'dashboards',
  templateUrl: './dashboards.component.html'
})
export class DashboardsComponent {
  public dashboards: Dashboard[] = [];
  public reportClass: string = "report-container";
  public phaseEmbedding: boolean = false;
  public embedConfig: EmbedConfig = {
    type: "dashboard",
    id: undefined,
    embedUrl: undefined,
    accessToken: undefined,
    tokenType: models.TokenType.Embed
  };

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    http.get<Dashboard[]>(baseUrl + 'api/dashboards/GetDashboards').subscribe(result => {
      this.dashboards = result;
      console.log(this.dashboards);
    }, error => console.error(error));
  }

  getDashboardConfig(event: any) {
    this.embedConfig.id = undefined;
    let dashboardId = event.target.value;
    this.http.get(this.baseUrl + `api/dashboards/GetDashboardConfig/${dashboardId}`).subscribe((result: any) => {
      this.embedConfig = {
        ...this.embedConfig,
        id: result?.dashboardId,
        embedUrl: result?.embedUrl,
        accessToken: result?.embedToken.token
      };
      console.log(this.embedConfig);
    }, error => console.error(error));
  }
}

interface Dashboard {
  id: string;
  displayName: string;
}

interface DashboardEmbedConfig {
  type: string,
  id?: string,
  embedUrl?: string,
  accessToken?: string,
  tokenType: number
}

