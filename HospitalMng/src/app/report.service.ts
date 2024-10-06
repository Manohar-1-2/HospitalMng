import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = 'http://localhost:5280/admin/downloadreport'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  downloadReport(fileName: string): Observable<Blob> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${localStorage.getItem('authToken')}`); // Assuming you're using JWT for authentication
    return this.http.get(`${this.apiUrl}/${fileName}`, { headers, responseType: 'blob' });
  }
}
