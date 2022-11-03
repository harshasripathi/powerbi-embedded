import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { PowerBIEmbedModule } from 'powerbi-client-angular';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ReportsComponent } from './reports/reports.component';
import { DashboardsComponent } from './dashboards/dashboards.component';
import { FiltersComponent } from './filters/filters.component';
import { LogsViewComponent } from './logs-view/logs-view.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ReportsComponent,
    DashboardsComponent,
    FiltersComponent,
    LogsViewComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    PowerBIEmbedModule,
    RouterModule.forRoot([
      { path: '', component: ReportsComponent, pathMatch: 'full' },
      { path: 'powerbi-reports', component: ReportsComponent },
      { path: 'powerbi-dashboards', component: DashboardsComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }

