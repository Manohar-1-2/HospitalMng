import { Component } from '@angular/core';
import { HttpClient,HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-add-docter',
  templateUrl: './add-docter.component.html',
  styleUrls: ['./add-docter.component.css']
})
export class AddDocterComponent {
  newDoctor = {
    firstName: '',
    lastName: '',
    speciality: '',
    location: '',
    educationQualification: '',
    yearsOfExperience: 0
  };

  private apiUrl = 'http://localhost:5280/admin/adddoctersDetails'; // Your backend API endpoint

  constructor(private http: HttpClient) {}

  // Function to handle form submission
  addDoctor() {
    console.log('New Doctor Details:', this.newDoctor);

    const token = localStorage.getItem('authToken'); // Get the token from local storage
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json' // Set content type if you're sending JSON
    });
    this.http.post(this.apiUrl, this.newDoctor,{headers}).subscribe(
      (response) => {
        console.log('Doctor added successfully', response);
        // Reset the form after submission
        this.resetForm();
      },
      (error) => {
        console.error('Error adding doctor:', error);
      }
    );
  }

  private resetForm() {
    this.newDoctor = {
      firstName: '',
      lastName: '',
      speciality: '',
      location: '',
      educationQualification: '',
      yearsOfExperience: 0
    };
  }
}
