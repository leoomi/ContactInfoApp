import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import Person from 'src/app/models/person';
import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
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
    private snackBar: MatSnackBar) {}

  people: Person[] | null = null;

  ngOnInit(): void {
    this.apiService.get<Person[]>("people")
      .subscribe({
        next: (people: Person[]) => {
          this.people = people;
        },
        error: (e: HttpErrorResponse) => {
          if (e.status === HttpStatusCode.Unauthorized) {
            this.signOutService.signOut();
            this.snackBar.open('Session expired. Please, log in again.', '', {
              duration: 5000,
            });

            return;
          }

          alert('Something went wrong!')
        }
      });
  }

  signOut(): void {
    this.signOutService.signOut();
  }
}
