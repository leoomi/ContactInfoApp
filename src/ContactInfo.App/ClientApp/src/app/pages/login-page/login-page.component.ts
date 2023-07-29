import { HttpErrorResponse, HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent {
  loginForm = this.fb.group({
    username: [null, Validators.required],
    password: [null, Validators.required],
  });

  hidePassword = true;

  constructor(private fb: FormBuilder,
    private apiService: ApiService,
    private router: Router) { }

  toggleVisibility() {
    this.hidePassword = !this.hidePassword;
  }

  onSubmit(): void {
    this.apiService.post('users/login', this.loginForm.value)
      .subscribe({
        next: (s: any) => {
          localStorage.setItem('token', s.token);
          this.router.navigate(['/']);
        },
        error: (e: HttpErrorResponse) => {
          if (e.status !== HttpStatusCode.Unauthorized) {
            alert('Something went wrong!');
            return;
          }

          this.loginForm.setErrors({
            serverErrors: 'Invalid user and password!'
          });
        }
      });
  }
}
