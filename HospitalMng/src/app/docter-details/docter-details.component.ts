import { Component, OnInit } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-docter-details',
  templateUrl: './docter-details.component.html',
  styleUrls: ['./docter-details.component.css']
})
export class DocterDetailsComponent implements OnInit {
  doctors: any[] = []; // Holds the doctor details

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getDoctorDetails();
  }

  getDoctorDetails() {
    // Replace with your API endpoint
    const token = localStorage.getItem('authToken'); // Get the token from local storage
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json' // Set content type if you're sending JSON
    });
    this.http.get<any[]>('http://localhost:5280/admin/getdoctersDetails',{headers}).subscribe(
      (response) => {
        this.doctors = response;
      },
      (error) => {
        console.error('Error fetching doctor details:', error);
      }
    );
  }
}
