import { Component, ElementRef, OnInit, ViewChild, AfterViewInit, Input } from "@angular/core";

@Component({
  selector: "logs-view",
  templateUrl: "./logs-view.component.html"
})

export class LogsViewComponent implements AfterViewInit {

  @ViewChild('logs')
  logs: ElementRef | undefined;

  @Input() incomingLogs!: string[];

  ngAfterViewInit() {
    setInterval(() => {
      if (this.logs?.nativeElement?.scrollTop)
        this.logs.nativeElement.scrollTop = this.logs?.nativeElement?.scrollHeight;
    }, 500);
  }
}
