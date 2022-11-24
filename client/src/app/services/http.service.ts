import {
  HttpClient,
  HttpParams,
  HttpHeaders,
  HttpErrorResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable()
export class HttpService {
  private apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) {}

  get<T>(
    resourcePath: string,
    headers?: HttpHeaders,
    params?: HttpParams
  ): Observable<T[]> {
    return this.http
      .get<T[]>(this.constructUrl(resourcePath, params), {
        headers: headers,
      })
      .pipe(catchError(this.handleError));
  }

  post<T>(
    resourcePath: string,
    resource: T,
    headers?: HttpHeaders,
    params?: HttpParams
  ): Observable<any> {
    return this.http
      .post(this.constructUrl(resourcePath, params), resource, {
        headers: headers,
      })
      .pipe(catchError(this.handleError));
  }

  put<T>(
    resourcePath: string,
    resource: T,
    headers?: HttpHeaders,
    params?: HttpParams
  ): Observable<any> {
    return this.http
      .put(this.constructUrl(resourcePath, params), resource, {
        headers: headers,
      })
      .pipe(catchError(this.handleError));
  }

  patch<T>(
    resourcePath: string,
    resource: T,
    headers?: HttpHeaders,
    params?: HttpParams
  ): Observable<any> {
    return this.http
      .patch(this.constructUrl(resourcePath, params), resource, {
        headers: headers,
      })
      .pipe(catchError(this.handleError));
  }

  private constructUrl(resourcePath: string, params?: HttpParams): string {
    console.log(this.apiUrl);
    let url = `${this.apiUrl}${resourcePath}`;

    if (params) return `${url}?${params.toString()}`;
    else return url;
  }

  private handleError(error: HttpErrorResponse) {
    return throwError(() => error);
  }
}
