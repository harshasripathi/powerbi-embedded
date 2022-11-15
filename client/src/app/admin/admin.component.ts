import { Component, OnInit } from "@angular/core";
import { AdminService, RequestConfig } from "../services/admin.service";

@Component({
    selector: "admin",
    templateUrl: "./admin.component.html"
})
export class AdminComponent implements OnInit {
    public requests: RequestConfig[] = [];

    constructor(private adminService: AdminService) {}

    ngOnInit(): void {
        this.adminService.requests.subscribe(i => this.requests = i);
    }
}