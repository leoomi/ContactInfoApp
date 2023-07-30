import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { faDiscord, faInstagram, faWhatsapp } from '@fortawesome/free-brands-svg-icons';
import { IconDefinition, faBug, faCoffee, faEnvelope, faPhone, faSuitcase } from '@fortawesome/free-solid-svg-icons';
import ContactType from 'src/app/models/contact-type';
import Person from 'src/app/models/person';
import { ApiService } from 'src/app/services/api.service';
import { SignOutService } from 'src/app/services/sign-out.service';

@Component({
  selector: 'app-person-details-page',
  templateUrl: './person-details-page.component.html',
  styleUrls: ['./person-details-page.component.css']
})
export class PersonDetailsPageComponent implements OnInit {
  constructor(private apiService: ApiService,
    private signOutService: SignOutService,
    private snackBar: MatSnackBar,
    private router: Router,
    private route: ActivatedRoute) {
    this.route.params.subscribe(params =>
      this.id = params['id']
    );
  }

  id: number | undefined;
  person: Person | undefined;
  ngOnInit(): void {
    this.apiService.get<Person>(`people/${this.id}`)
      .subscribe({
        next: (person: Person) => {
          this.person = person;
        },
        error: (e: HttpErrorResponse) => {
          if (e.status === HttpStatusCode.Unauthorized) {
            this.signOutService.signOut();
            this.snackBar.open('Session expired. Please, log in again.', '', {
              duration: 5000,
            });

            return;
          }
          alert('Something went wrong!');
        }
      });
  }

  getIcon(type: ContactType | undefined): IconDefinition {
    switch (type) {
      case ContactType.Phone:
        return faPhone;
      case ContactType.CompanyPhone:
        return faSuitcase;
      case ContactType.Whatsapp:
        return faWhatsapp;
      case ContactType.Email:
        return faEnvelope;
      case ContactType.Discord:
        return faDiscord;
      case ContactType.Instagram:
        return faInstagram;
      default:
        return faBug;
    }
  }

  delete() {
    this.apiService.delete(`people/${this.id}`)
      .subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        error: (e: HttpErrorResponse) => {
          if (e.status === HttpStatusCode.Unauthorized) {
            this.signOutService.signOut();
            this.snackBar.open('Session expired. Please, log in again.', '', {
              duration: 5000,
            });

            return;
          }
          alert('Something went wrong!');
        }
      });
  }
}
