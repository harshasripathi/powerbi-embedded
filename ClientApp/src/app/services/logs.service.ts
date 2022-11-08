import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class LogService {
  private logs$ = new BehaviorSubject<string>("");
  public logs = this.logs$.asObservable();

  pushLog(log: string) {
    this.logs$.next(log);
  }
}