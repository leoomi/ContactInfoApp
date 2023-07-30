import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.baseUrl = baseUrl + 'api/';
  }

  get<T>(url: string): Observable<T> {
    return this.http.get<T>(this.baseUrl + url, this.buildOptionsWithAuthorization());
  }

  post<T1, T2>(url: string, body: T1): Observable<T2> {
    return this.http.post<T2>(this.baseUrl + url, body, this.buildOptionsWithAuthorization());
  }

  delete<T>(url: string): Observable<T> {
    return this.http.delete<T>(this.baseUrl + url, this.buildOptionsWithAuthorization());
  }

  buildOptionsWithAuthorization(): { headers: HttpHeaders } {
    const token = localStorage.getItem('token');
    if (!token) {
      return { headers: new HttpHeaders() };
    }

    return {
      headers: new HttpHeaders({
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      })
    };
  }
}
