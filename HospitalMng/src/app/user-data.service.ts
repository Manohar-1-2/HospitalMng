import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UserDataService {
  private userData: any;

  // Set the selected user data
  setUser(data: any) {
    this.userData = data;
  }

  // Get the selected user data
  getUser() {
    return this.userData;
  }
}
