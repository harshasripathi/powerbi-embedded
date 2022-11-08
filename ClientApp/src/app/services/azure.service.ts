import { HttpClient } from '@angular/common/http';
import { Injectable } from "@angular/core";

@Injectable()
export class AzureService {
  constructor(private httpClient: HttpClient) { }
  private readonly azureUrl: string = "https://hs-testservice.azurewebsites.net/api/RequestTrigger?code=znCM6tp0IFOtuMiQu2W_AX-8aCkQ94XnKnNghEE4oxKGAzFujYHYiw==";

  pushLogsToCloud(logs: any) {
    console.log(logs);
    this.httpClient.post(this.azureUrl, JSON.stringify(logs)).subscribe(result => {
      console.log("Azure function result => ", result)
    }, error => console.log("Azure function error => ", error));
  }
}
