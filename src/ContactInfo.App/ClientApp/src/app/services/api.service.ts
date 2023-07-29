import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.baseUrl = baseUrl + 'api/';
  }

  get<T>(url: string): Observable<T> {
    return this.http.get<T>(this.baseUrl + url);
  }

  post<T1, T2>(url: string, body: T1): Observable<T2> {
    return this.http.post<T2>(this.baseUrl + url, body);
  }

  delete<T>(url: string): Observable<T> {
    return this.http.get<T>(this.baseUrl + url);
  }
}
