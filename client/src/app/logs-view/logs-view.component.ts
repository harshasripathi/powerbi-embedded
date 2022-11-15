import { Component, ElementRef, OnInit, ViewChild, AfterViewInit, Input } from "@angular/core";
import { AzureService } from '../services/azure.service';
import { LogService } from '../services/logs.service';

@Component({
  selector: "logs-view",
  templateUrl: "./logs-view.component.html"
})

export class LogsViewComponent implements OnInit, AfterViewInit {

  constructor(private logService: LogService, private azureService: AzureService) { }

  @ViewChild('logs')
  logs: ElementRef | undefined;

  public incomingLogs: string[] = [];

  ngOnInit() {
    this.logService.logs.subscribe(val => {
      if (val && val !== "") {
        this.azureService.pushLogsToCloud(val);
        this.incomingLogs.push(val);
      }
    });
  }

  ngAfterViewInit() {
    setInterval(() => {
      if (this.logs?.nativeElement?.scrollTop)
        this.logs.nativeElement.scrollTop = this.logs?.nativeElement?.scrollHeight;
    }, 500);
  }

  clear() {
    this.incomingLogs = [];
  }
}
