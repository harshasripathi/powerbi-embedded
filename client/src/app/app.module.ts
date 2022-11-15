import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { PowerBIEmbedModule } from 'powerbi-client-angular';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown'
import { MsalGuard, MsalInterceptor, MsalModule } from '@azure/msal-angular';
import { PublicClientApplication, InteractionType, BrowserCacheLocation } from '@azure/msal-browser';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ReportsComponent } from './reports/reports.component';
import { DashboardsComponent } from './dashboards/dashboards.component';
import { FiltersComponent } from './filters/filters.component';
import { LogsViewComponent } from './logs-view/logs-view.component';
import { FilterService } from './services/filter.service';
import { LogService } from './services/logs.service';
import { AzureService } from './services/azure.service';
import { LoaderService } from './services/loader.service';
import { SpinnerComponent } from './shared/spinner/spinner.component';
import { AccessRequestComponent } from './access-request/access.request.component';
import { AdminComponent } from './admin/admin.component';
import { PowerBIService } from './services/powerbi.service';
import { AdminService } from './services/admin.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ReportsComponent,
    DashboardsComponent,
    FiltersComponent,
    LogsViewComponent,
    SpinnerComponent,
    AccessRequestComponent,
    AdminComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    PowerBIEmbedModule,
    NgMultiSelectDropDownModule,
    RouterModule.forRoot([
      {path: '', canActivate: [MsalGuard], children: [
        { path: '', component: ReportsComponent, pathMatch: 'full' },
        { path: 'powerbi-reports', component: ReportsComponent },
        { path: 'request', component: AccessRequestComponent},
        { path: 'admin', component: AdminComponent }
      ]}
    ]),
    MsalModule.forRoot(new PublicClientApplication(
      {
        auth: {
          clientId: "8318fff9-84ad-4293-ad06-b477af645f65",
          authority:
            'https://login.microsoftonline.com/24041256-c834-468a-b7e7-c4dc34322cfe',
          redirectUri: "/"
        },
        cache: {
          cacheLocation: BrowserCacheLocation.SessionStorage,
          storeAuthStateInCookie: true,
        }
      }),
      {
        // MSAL Guard Configuration
        interactionType: InteractionType.Redirect,
        authRequest: {
            scopes: ['']
        },
        loginFailedRoute: '/'
      },
      {
        interactionType: InteractionType.Redirect,
        protectedResourceMap: new Map([
          [
            "/api/", ["api://4a193b8b-e8fd-4dcd-9d16-132cba83f3f2/Read.All"]
          ]
        ])
      })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    FilterService, 
    LogService, 
    AzureService, 
    LoaderService,
    PowerBIService,
    AdminService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

