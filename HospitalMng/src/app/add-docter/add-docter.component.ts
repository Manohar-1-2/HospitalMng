import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-docter',
  templateUrl: './add-docter.component.html',
  styleUrls: ['./add-docter.component.css']
})
export class AddDocterComponent {
  addDoctorForm: FormGroup;
  private apiUrl = 'http://localhost:5280/admin/adddoctersDetails'; // Your backend API endpoint

  constructor(private http: HttpClient, private fb: FormBuilder) {
    this.addDoctorForm = this.fb.group({
      firstName: [
        '',
        [Validators.required, Validators.pattern(/^[A-Za-z]+$/)] // No numbers allowed
      ],
      lastName: [
        '',
        [Validators.required, Validators.pattern(/^[A-Za-z]+$/)] // No numbers allowed
      ],
      speciality: ['', Validators.required],
      location: [
        '',
        [Validators.required, Validators.pattern(/^[A-Za-z\s]+$/)] // No numbers allowed, spaces allowed
      ],
      educationQualification: ['', Validators.required],
      yearsOfExperience: [0, [Validators.required, Validators.min(0)]]
    });
  }

  // Function to handle form submission
  addDoctor() {
    if (this.addDoctorForm.invalid) {
      console.log('Form is invalid');
      return;
    }

    const token = localStorage.getItem('authToken'); // Get the token from local storage
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json' // Set content type if you're sending JSON
    });

    this.http.post(this.apiUrl, this.addDoctorForm.value, { headers }).subscribe(
      (response) => {
        console.log('Doctor added successfully', response);
        // Reset the form after submission
        this.addDoctorForm.reset();
      },
      (error) => {
        console.error('Error adding doctor:', error);
      }
    );
  }
}
