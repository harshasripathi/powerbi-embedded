import { Injectable } from "@angular/core";
import { IFilterConfig } from "../models/filter.config"

@Injectable()
export class FilterService {
  filterConfig: IFilterConfig[] =
    [{
      reportId: "4a544318-a582-4919-af80-a500e4924609",
      tableName: "LocalDateTable_b0a709bf-debc-4c4a-8a3f-31176c67b753",
      column: {
        name: "Month",
        values: [
          "January",
          "February",
          "March",
          "April",
          "May",
          "June"
        ],
        isMultiSelect: true
      }
    },
    {
      reportId: "4a544318-a582-4919-af80-a500e4924609",
      tableName: "logdata (2)",
      column: {
        name: "Status",
        values: [
          "Active",
          "Accepted",
          "Canceled",
          "Completed",
          "Failed",
          "Resolved",
          "Started",
          "Succeeded",
          "Updated"
        ],
        isMultiSelect: true
      }
    }];

  getFilterConfig(reportId: string): IFilterConfig[] {
    return this.filterConfig.filter(i => i.reportId == reportId);
  }
}
