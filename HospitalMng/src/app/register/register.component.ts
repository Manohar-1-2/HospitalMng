import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router'; 
interface RegisterResponse {
  userid: string; // Adjust the type based on what your backend returns
}
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl:'./register.component.css'
})

export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder,private http: HttpClient,private router: Router) {
    this.registerForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName: ['', [Validators.required, Validators.minLength(2)]],
      dob: ['', Validators.required],
      mobile: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      address: ['', Validators.required],
      state: ['', Validators.required],
      country: ['', Validators.required],
      relativeName: ['', Validators.required],
      relativeNumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]],
      illnessDetails: [''],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, this.matchPasswords('password')]]
    });
  }

  matchPasswords(passwordKey: string) {
    return (control: any) => {
      if (this.registerForm) {
        const password = this.registerForm.get(passwordKey);
        if (password && control.value !== password.value) {
          return { mismatch: true };
        }
      }
      return null;
    };
  }

  onSubmit() {
    console.log("hai....")
    if (this.registerForm.valid) {

      
      // Create a copy of the form values and omit confirmPassword
      const { confirmPassword, ...userData } = this.registerForm.value;

       // Log user data without confirmPassword

      // Make POST request
      this.http.post<RegisterResponse>('http://localhost:5280/register', userData)
        .subscribe({
          next: (response) => {
            console.log('User registered successfully', response);
            alert(`Registration sucessfull.. userID:${response.userid}`);
            this.router.navigate(['/login']);
          },
          error: (error) => {
            console.error('Error registering user', error);
          }
        });
    } else {
      console.log('Form is not valid!');
    }
  }
}
