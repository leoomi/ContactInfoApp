import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class SignOutService {
  constructor(private router: Router) {}

  signOut() {
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
