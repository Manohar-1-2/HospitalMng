import { Component } from '@angular/core';
import {  OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserDataService } from '../user-data.service';
import { HttpClient,HttpHeaders } from '@angular/common/http';
@Component({
  selector: 'app-edit-page',
  templateUrl: './edit-page.component.html',
  styleUrl: './edit-page.component.css'
})
export class EditPageComponent {
  patient: any = {
    userUID: '',
    firstName: '',
    lastName: '',
    dob: new Date(),
    mobile: '',
    address: '',
    state: '',
    country: '',
    relativeName: '',
    relativeNumber: '',
    illnessDetails: ''
  };
  constructor( private userDataService: UserDataService,private http: HttpClient,private router: Router) {}

  ngOnInit(): void {
    // Get the user data from the service
    this.patient = this.userDataService.getUser();

    if (!this.patient) {
      // If the data is not available (e.g., a direct page load), handle the error
      console.error("No patient data found.");
    }
  }
  
 

  updatePatient() {
    const apiUrl = "http://localhost:5280/admin/editpatientdetials";
  
    // Retrieve the token from local storage (or any other storage mechanism)
    const token = localStorage.getItem('authToken'); // Adjust if you're using session storage or another method
  
    // Set the headers with the Authorization Bearer token
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}` // Add the JWT token here
    });
  
    // Send the HTTP request with headers
    this.http.post(apiUrl, this.patient, { headers }).subscribe({
      next: (response) => {
        console.log('Patient updated successfully:', response);
        alert("Patient Details Sucessfully Updated...");
        this.router.navigate(['/adminPage']);
        // Handle success (e.g., show success message or redirect)
      },
      error: (error) => {
        console.error('Error updating patient:', error);
        // Handle error (e.g., show error message)
      }
    });
  }
  
}
