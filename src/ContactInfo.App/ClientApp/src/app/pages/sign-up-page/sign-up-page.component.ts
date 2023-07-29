import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-sign-up-page',
  templateUrl: './sign-up-page.component.html',
  styleUrls: ['./sign-up-page.component.css']
})
export class SignUpPageComponent {
  signUpForm = this.fb.group({
    username: [null, Validators.required],
    password: [null, Validators.required],
    passwordConfirmation: [null, Validators.required],
    genericError: [null]
  });

  hidePassword = true;
  hideConfirmation = true;

  constructor(private fb: FormBuilder,
    private snackBar: MatSnackBar,
    private apiService: ApiService,
    private router: Router) { }

  togglePasswordVisibility() {
    this.hidePassword = !this.hidePassword;
  }

  toggleConfirmationVisibility() {
    this.hideConfirmation = !this.hideConfirmation;
  }

  onSubmit(): void {
    this.apiService.post('users/', this.signUpForm.value)
      .subscribe({
        next: (s: any) => {
          this.snackBar.open("User created! Please login.", "", {
            duration: 3000
          });
          this.router.navigate(['/login']);
        },
        error: (e: HttpErrorResponse) => {
          if (e.status !== HttpStatusCode.BadRequest) {
            alert('Something went wrong!');
            return;
          }

          const validationErrors = e.error.errors;
          Object.keys(validationErrors).forEach((key) => {
            const field = this.signUpForm.get(key);
            if (!field) {
              return
            }

            field.setErrors({
              serverError: validationErrors[key]
            })
          });
        }
      });
  }
}
