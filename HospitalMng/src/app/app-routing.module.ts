import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { AdminPageComponent } from './admin-page/admin-page.component';
import { PatientPageComponent } from './patient-page/patient-page.component';
import { AuthGuard } from './auth.guard';
import { EditPageComponent } from './edit-page/edit-page.component';
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'adminPage', component: AdminPageComponent, canActivate: [AuthGuard] },
  { path: 'patientPage', component: PatientPageComponent,canActivate: [AuthGuard]},
  { path: 'adminPage/edit', component: EditPageComponent,canActivate: [AuthGuard]} 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
