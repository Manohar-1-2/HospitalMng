import { Component, OnInit } from '@angular/core';

import { HttpClient ,HttpHeaders} from '@angular/common/http';
import { Router } from '@angular/router';
import { UserDataService } from '../user-data.service';
import { ReportService } from '../report.service';
interface Report {
  title: string;
  file: File | null ;
  userId:string;
}
@Component({
  selector: 'app-patient-details',
  templateUrl: './patient-details.component.html',
  styleUrl: './patient-details.component.css'
})
export class PatientDetailsComponent implements OnInit {
  data:any; // Declare a property to store fetched data
  newReport: Report = { title: ' ', file: null ,userId:''};
  reports:Report[]=[];

  constructor(private http: HttpClient,private router: Router,private userDataService: UserDataService,private reportService :ReportService) {}

  ngOnInit(): void {
    this.fetchData(); 

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
  onFileChange(event: any, userUID: string) {
    const file = event.target.files[0];
    if (file && file.type === 'application/pdf') {
      this.newReport.file = file;
    } else {
      alert('Please upload a valid PDF file.');
    }
  }

  // Handle report upload
  uploadReport(userUID: string) {
    const apiUrl = `http://localhost:5280/admin/uploadreports`;
    this.newReport.userId=userUID;
  
    if (!this.newReport.title || !this.newReport.file) {
      alert('Please provide both a title and a file.');
      return;
    }

    const formData = new FormData();
    formData.append('title', this.newReport.title);
    formData.append('pdf', this.newReport.file as Blob); 
    formData.append('UserUID', this.newReport.userId);
    this.reports.push({...this.newReport})
    this.newReport.title=''
    this.newReport.file=null;
    const authToken = localStorage.getItem('authToken'); // Ensure you use the correct key

  // Set HTTP headers with the token
  const headers = new HttpHeaders({
    'Authorization': `Bearer ${authToken}`
  });
    this.http.post(apiUrl, formData, { headers }).subscribe({
      next: (response: any) => {
        console.log('Report uploaded successfully:', response);
        this.fetchData();
        alert("Report uploaded sucessfully..")

      },
      error: (error) => {
        console.error('Error uploading report:', error);
      }
    });
  }
  downloadReport(fileName: string) {
    console.log(fileName)
    this.reportService.downloadReport(fileName).subscribe(
      (response: Blob) => {
        const fileUrl = window.URL.createObjectURL(response);
        const a = document.createElement('a');
        a.href = fileUrl;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(fileUrl); // Clean up the URL object after download
      },
      error => {
        console.error('Error downloading the report', error);
      }
    );
  }
  editUser(user: any) {
    this.userDataService.setUser(user);
    this.router.navigate(['adminPage/edit']);
    console.log("Edit user:", user);
  }
}
