import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminPageComponent } from './admin-page/admin-page.component';
import { PatientPageComponent } from './patient-page/patient-page.component';
import { EditPageComponent } from './edit-page/edit-page.component';
import { PatientDetailsComponent } from './patient-details/patient-details.component';
import { DocterDetailsComponent } from './docter-details/docter-details.component';
import { AddDocterComponent } from './add-docter/add-docter.component';
import { AppointmentsComponent } from './appointments/appointments.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    AdminPageComponent,
    PatientPageComponent,
    EditPageComponent,
    PatientDetailsComponent,
    DocterDetailsComponent,
    AddDocterComponent,
    AppointmentsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
