import { Component, OnInit } from '@angular/core';
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';
import { ApiService } from 'src/app/services/api.service';
import { Router } from '@angular/router';
import Person from 'src/app/models/person';
import { HttpErrorResponse } from '@angular/common/http';
import { SignOutService } from 'src/app/services/sign-out.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-contact-list-page',
  templateUrl: './contact-list-page.component.html',
  styleUrls: ['./contact-list-page.component.css']
})
export class ContactListPageComponent implements OnInit {
  constructor(private apiService: ApiService,
    private signOutService: SignOutService,
    private snackBar: MatSnackBar,
    private router: Router) {}

  ngOnInit(): void {
    this.apiService.get<Person[]>("/people")
      .subscribe({
        next: (people: Person[]) => {
          console.log(people);
        },
        error: (e: HttpErrorResponse) => {
          console.log(e);
          this.signOutService.signOut();
          this.snackBar.open('Session expired. Please, log in again.', '', {
            duration: 5000,
          });
        }
      });
  }

  signOut(): void {
    this.signOutService.signOut();
  }
}
