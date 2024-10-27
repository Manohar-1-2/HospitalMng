import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AdminPageComponent } from './admin-page/admin-page.component';
import { PatientPageComponent } from './patient-page/patient-page.component';
import { AuthGuard } from './auth.guard';
import { EditPageComponent } from './edit-page/edit-page.component';
import { PatientDetailsComponent } from './patient-details/patient-details.component';
import { DocterDetailsComponent } from './docter-details/docter-details.component';
import { AddDocterComponent } from './add-docter/add-docter.component';
import { AppointmentsComponent } from './appointments/appointments.component';
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  {
    path: 'adminPage', component: AdminPageComponent, canActivate: [AuthGuard], children: [
      { path: 'patient-details', component: PatientDetailsComponent},
      { path: 'doctor-details', component: DocterDetailsComponent },
      { path: 'add-docter', component: AddDocterComponent },
      { path: 'add-patient', component: RegisterComponent },
      { path: 'edit', component: EditPageComponent},
      { path: 'appointments', component: AppointmentsComponent},
      { path: '', redirectTo: 'patient-details', pathMatch: 'full' }
    ]
  },
  { path: 'patientPage', component: PatientPageComponent,canActivate: [AuthGuard]},
 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
