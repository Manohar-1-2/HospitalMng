import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
interface LoginResponse {
  token: string;
  role: string ;
}
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(private fb: FormBuilder,private http: HttpClient,private router: Router) {
    this.loginForm = this.fb.group({
      UserUID: ['', [Validators.required, Validators.minLength(3)]],
      Password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      console.log("hai....",this.loginForm.value)
      this.http.post<LoginResponse>('http://localhost:5280/login', this.loginForm.value)
        .subscribe({
          next: (response) => {
            console.log('User successfully logged in ', response.token);
            localStorage.setItem('authToken', response.token);
            localStorage.setItem('userRole', response.role);
            
            if (response.role === 'admin') {
              this.router.navigate(['/adminPage']);
            } else if (response.role === 'patient') {
              this.router.navigate(['/patientPage']);
              console.log(response.role)
            }
          },
          error: (error) => {
            console.error('Error logging user', error);
          }
        });
    } else {
      console.log('Form is not valid!');
    }
  }
}
