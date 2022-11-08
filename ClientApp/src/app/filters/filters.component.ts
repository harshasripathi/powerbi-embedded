import { AfterViewInit, Component, Input, OnChanges, SimpleChanges } from "@angular/core";
import { PowerBIReportEmbedComponent } from 'powerbi-client-angular';
import { Report, models, VisualDescriptor } from 'powerbi-client';
import { AdvancedFilter, AdvancedFilterLogicalOperators } from 'powerbi-models';


import { FilterService } from "../services/filter.service";
import { IFilterConfig } from '../models/filter.config';
import { LogService } from '../services/logs.service';

@Component({
  selector: "filters",
  templateUrl: "./filters.component.html"
})

export class FiltersComponent implements OnChanges {

  constructor(private filterService: FilterService, private logService: LogService) { }

  @Input() reportObj!: PowerBIReportEmbedComponent;
  private report!: Report;
  public filterConfig!: IFilterConfig[];
  public dropdownSettings: any = {};
  public data: any;
  public settings: any;
  public advancedFilters: any[] = [];
  readonly advancedFilterSchemaUrl: string = "http://powerbi.com/product/schema#advanced";

  ngOnChanges(changes: SimpleChanges) {
    console.log(changes);
    if (changes.reportObj.currentValue) {
      this.report = this.reportObj.getReport();
      this.filterConfig = this.filterService.getFilterConfig(this.report.getId());
    }
  }

  async onDropdownChange(config: IFilterConfig, event: any) {
    this.advancedFilters = [];
    this.advancedFilters.push({
      target: {
        table: config.tableName,
        column: config.column.name
      },
      schemaUrl: this.advancedFilterSchemaUrl,
      filterType: models.FilterType.Advanced,
      logicalOperator: "And",
      conditions: [
        {
          operator: "Is",
          value: event?.target?.value
        }
      ]
    });

    this.logService.pushLog(`Applied filter : ${JSON.stringify(this.advancedFilters)}`);
    await this.report.updateFilters(models.FiltersOperations.Replace, this.advancedFilters);
  }

  async clear() {
    this.logService.pushLog("Cleared all the filters");
    await this.report.updateFilters(models.FiltersOperations.RemoveAll);

    let pages = await this.report.getPages();
    console.log(pages);
    let visuals: VisualDescriptor[] = [];
    for (let page of pages) {
      let iVisuals = await page.getVisuals();
      visuals = visuals.concat(iVisuals);
    }
    console.log(visuals);
    for (let visual of visuals) {
      await visual.updateFilters(models.FiltersOperations.RemoveAll);
    }
  }
}
