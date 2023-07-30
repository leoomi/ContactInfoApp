import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { IconDefinition, faWhatsapp } from '@fortawesome/free-brands-svg-icons';
import { faEnvelope, faPhone, faSuitcase } from '@fortawesome/free-solid-svg-icons';
import ContactType from 'src/app/models/contact-type';
import Person from 'src/app/models/person';
import { ApiService } from 'src/app/services/api.service';
import { SignOutService } from 'src/app/services/sign-out.service';

type ContactForm = FormGroup<{
  id: FormControl<number | null>,
  personId: FormControl<number | null>,
  info: FormControl<string | null>,
  type: FormControl<ContactType | null>
}>;

@Component({
  selector: 'app-person-form',
  templateUrl: './person-form.component.html',
  styleUrls: ['./person-form.component.css']
})
export class PersonFormComponent implements OnInit {
  personForm = new FormGroup({
    id: new FormControl<number | null>(null),
    userId: new FormControl<number | null>(null),
    firstName: new FormControl<string | null>(null, Validators.required),
    lastName: new FormControl<string | null>(null, Validators.required),
    contacts: new FormArray<ContactForm>([]),
  });

  contactTypeOptions = [
    { type: ContactType.Phone, icon: faPhone },
    { type: ContactType.CompanyPhone, icon: faSuitcase },
    { type: ContactType.Whatsapp, icon: faWhatsapp },
    { type: ContactType.Email, icon: faEnvelope },
  ];

  id: number | undefined;
  showSpinner = true;

  constructor(private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private signOutService: SignOutService,
    private apiService: ApiService,
    private router: Router,
    private route: ActivatedRoute) {
    this.route.params.subscribe(params => {
      this.id = params['id'];
      this.showSpinner = false;
    });
  }

  ngOnInit(): void {
    this.addContactForm();
    if (this.isEdit()) {
      this.apiService.get<Person>(`people/${this.id}`)
        .subscribe({
          next: (person: Person) => {
            this.setFormValues(person);
          },
          error: this.onError
        });
    }
  }

  isEdit(): boolean {
    return !!this.id;
  }

  addContactForm(): void {
    const contactForm: ContactForm = new FormGroup({
      id: new FormControl<number | null>(null),
      personId: new FormControl<number | null>(null),
      info: new FormControl<string | null>(null, Validators.required),
      type: new FormControl<ContactType | null>(ContactType.Phone, Validators.required),
    });
    this.getContactsField().push(contactForm);
  }

  deleteContact(index: number): void {
    this.getContactsField().removeAt(index);
  }

  getContactsField(): FormArray<ContactForm> {
    return this.personForm.controls['contacts'] as FormArray;
  }

  getSelectedIcon(form: ContactForm): IconDefinition {
    const type = form.controls['type'].value as ContactType;
    return this.contactTypeOptions[type - 1].icon;
  }

  onSubmit(): void {
    console.log(JSON.stringify(this.personForm.value));
    if (this.personForm.invalid) {
      return;
    }

    const person = this.personForm.value;
    if (!this.isEdit()) {
      this.apiService.post<unknown, Person>(`people/`, person)
        .subscribe({
          next: (person: Person) => {
            this.router.navigate(['/']);
          },
          error: this.onError
        });
        return;
    }

    this.apiService.post<unknown, Person>(`people/${this.id}`, person)
        .subscribe({
          next: (person: Person) => {
            this.router.navigate(['/person', this.id]);
          },
          error: this.onError
        });
  }

  onError(e: HttpErrorResponse) {
    if (e.status === HttpStatusCode.Unauthorized) {
      this.signOutService.signOut();
      this.snackBar.open('Session expired. Please, log in again.', '', {
        duration: 5000,
      });

      return;
    }
    alert('Something went wrong!');
  }

  setFormValues(person: Person) {
    const contacts = person.contacts?.map((c) => {
      return {
        id: c.id!,
        personId: c.personId!,
        type: c.type!,
        info: c.info!
      }
    });
    this.personForm.setValue({
      id: person.id!,
      userId: person.userId!,
      firstName: person.firstName!,
      lastName: person.lastName!,
      contacts: contacts!
    });
  }
}
