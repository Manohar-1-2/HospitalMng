import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());

  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor() {}

  // Check if token exists
  hasToken(): boolean {
    return !!localStorage.getItem('authToken');
  }

  login() {
    this.isLoggedInSubject.next(true); // Update the login status
  }

  logout() {
    localStorage.removeItem('authToken');
    this.isLoggedInSubject.next(false); // Update the login status
  }
}
