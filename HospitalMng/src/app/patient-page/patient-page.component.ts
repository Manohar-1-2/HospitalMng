import { Component } from '@angular/core';
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
  selector: 'app-patient-page',
  templateUrl: './patient-page.component.html',
  styleUrl: './patient-page.component.css'
})
export class PatientPageComponent {
  user:any; // Declare a property to store fetched data
  newReport: Report = { title: ' ', file: null ,userId:''};
  reports:Report[]=[];

  constructor(private http: HttpClient,private router: Router,private userDataService: UserDataService,private reportService :ReportService) {}

  ngOnInit(): void {
    this.fetchData(); 

  }

  fetchData(): void {
    const token = localStorage.getItem('authToken'); 
    const headers = { Authorization: `Bearer ${token}` }; 
    this.http.get('http://localhost:5280/patient', { headers }).subscribe({
      next: (response:any) => {
        this.user = response; 
        console.log('Fetched data:', this.user);
      },
      error: (err:any) => {
        console.error('Error fetching data:', err); 
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
      (error:any) => {
        console.error('Error downloading the report', error);
      }
    );
  }

}
