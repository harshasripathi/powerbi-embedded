import { AfterViewInit, Component, Input } from "@angular/core";
import { PowerBIReportEmbedComponent } from 'powerbi-client-angular';
import { Report } from 'powerbi-client';

@Component({
  selector: "filters",
  templateUrl: "./filters.component.html"
})

export class FiltersComponent implements AfterViewInit {
  @Input() reportObj!: PowerBIReportEmbedComponent;
  private report!: Report;

  ngAfterViewInit() {
    setTimeout(() => console.log(this.reportObj.getReport()), 5000);
  }
}
