import { Component, OnInit } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { UserDataService } from '../user-data.service';
@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent implements OnInit {

  data: any; // Declare a property to store fetched data

  constructor(private http: HttpClient,private router: Router,private userDataService: UserDataService) {}

  ngOnInit(): void {
    this.fetchData(); // Call the fetchData method when the component is initialized
  }

  fetchData(): void {
    const token = localStorage.getItem('authToken'); 
    const headers = { Authorization: `Bearer ${token}` }; 
    this.http.get('http://localhost:5280/admin', { headers }).subscribe({
      next: (response) => {
        this.data = response; 
        console.log('Fetched data:', this.data);
      },
      error: (err) => {
        console.error('Error fetching data:', err); 
      }
    });
  }
  editUser(user: any) {
    this.userDataService.setUser(user);
    this.router.navigate(['/adminPage/edit']);
    console.log("Edit user:", user);
  }
}
