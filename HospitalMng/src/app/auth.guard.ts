import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const token = localStorage.getItem('authToken');
    const role = localStorage.getItem('userRole');

    if (token) {
      const routeUrl = route.url.toString(); // Get the upcoming route URL as a string

      // Check if the user is authorized for this route based on their role
      
      if (role === 'admin' && routeUrl.includes('adminPage')) {
        return true;
      } else if (role === 'patient' && routeUrl.includes('patientPage')) {
        return true;
      } else {
        this.router.navigate(['/login']);
        return false;
      }
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
