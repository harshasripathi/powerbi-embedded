import { Injectable, OnDestroy } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable()
export class AdminService {
    private _requests = new BehaviorSubject<RequestConfig[]>([]);
    public requests = this._requests.asObservable();

    reset() {
        console.log("On service destroy");  
        //this._requests.complete();
    }

    addRequest(request: RequestConfig) {
        let requests = this._requests.getValue();
        if(requests.filter(i => i.Data.ReportId == request.Data.ReportId).length == 0){
            this._requests.next(this._requests.getValue().concat(request));
        }
    }

    removeRequest(request: RequestConfig) {
        let requests = this._requests.getValue();
        this._requests.next(requests.filter(i => i != request));
    }
}

export interface RequestConfig {
    UserId: string,
    Data: {
        RequestedUser: string,
        ReportId: string,
        ReportName: string,
        ExpiryDate: string
    }
}