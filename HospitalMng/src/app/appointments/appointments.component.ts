import { Component } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient,HttpHeaders } from '@angular/common/http';
@Component({
  selector: 'app-appointments',
  templateUrl: './appointments.component.html',
  styleUrl: './appointments.component.css'
})
export class AppointmentsComponent {
  searchPatientForm: FormGroup;
  searchDocterForm: FormGroup;
  getSlotForm: FormGroup;
  patients: any[] | null = null;
  docters: any[] | null = null;
  slots: any[] | null = null;
  showdocterform :boolean=false;
  constructor(private fb: FormBuilder, private http: HttpClient) {
    this.searchPatientForm = this.fb.group({
      useruid: ['']
    });
    this.searchDocterForm = this.fb.group({
      location: [''],
      specialty:['']
    });
    this.getSlotForm = this.fb.group({
      date: Date
    });
  }

  onUserSearch() {
    const useruid = this.searchPatientForm.get('useruid')?.value;
    const token = localStorage.getItem('authToken'); 

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    this.http.get<any[]>(`http://localhost:5280/admin/searchpatients?useruid=${useruid}`,{headers}).subscribe(
      (response:any) => {
        this.patients = response;
        console.log(this.patients)
      },
      (error:any) => {
        console.error('Error fetching patients', error);
      }
    );
  }
  onDoctersSearch(){
    const sp = this.searchDocterForm.get('specialty')?.value;
    const loc = this.searchDocterForm.get('location')?.value;
    const token = localStorage.getItem('authToken'); 

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    this.http.get<any[]>(`http://localhost:5280/admin/searchdoctors?specialty=${sp}&location=${loc}`,{headers}).subscribe(
      (response:any) => {
        this.docters = response;
        console.log(this.docters)
      },
      (error:any) => {
        console.error('Error fetching patients', error);
      }
    );
  }
  onappointment(){
    this.showdocterform=!this.showdocterform;
  }
  getslots(doctorId:string){
      const date = this.getSlotForm.get('date')?.value;
      const token = localStorage.getItem('authToken'); 
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      this.http.get<any[]>(`http://localhost:5280/admin/getslots?doctorId=${doctorId}&date=${date}`,{headers})
        .subscribe(
          (response: any[]) => {
            this.slots = response; // Store the fetched slots
            console.log(this.slots)
          },
          (error) => {
            console.error('Error fetching slots', error);
            alert("No slots found...")
            
          }
        );
    }
    bookslot(id:string,useruid:string){
    
      const token = localStorage.getItem('authToken'); 
      const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      this.http.get<any[]>(`http://localhost:5280/admin/book?userId=${useruid}&slotId=${id}`,{headers})
        .subscribe(
          (response: any[]) => {
            this.slots = response; // Store the fetched slots
            console.log(this.slots) 
            alert("sucessfully booked")
          },
          (error) => {
            console.error('Error fetching slots', error);
            
          }
        );
    }
}
