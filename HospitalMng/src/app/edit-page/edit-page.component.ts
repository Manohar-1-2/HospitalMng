import { Component } from '@angular/core';
import {  OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UserDataService } from '../user-data.service';
@Component({
  selector: 'app-edit-page',
  templateUrl: './edit-page.component.html',
  styleUrl: './edit-page.component.css'
})
export class EditPageComponent {
  patient: any = {};  // To store patient data

  constructor(private route: ActivatedRoute, private userDataService: UserDataService) {}

  ngOnInit(): void {
    // Get the user data from the service
    this.patient = this.userDataService.getUser();

    if (!this.patient) {
      // If the data is not available (e.g., a direct page load), handle the error
      console.error("No patient data found.");
    }
  }
  updatePatient() :void{
    
  }
}
