import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../services/auth';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { LoaderService } from '../../shared/services/loader';
import { delay } from 'rxjs';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private loader = inject(LoaderService);

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  errorMessage = signal<string | null>(null);
  onSubmit() {
    if (this.loginForm.invalid) return;
    this.errorMessage.set(null);

    this.loader.show();

    this.auth.login(this.loginForm.value)
    .pipe(delay(500))
    .subscribe({
      next: (res) => {
        console.log('Logged in', res);
        this.errorMessage.set(null);
        this.router.navigate(['/newsfeed']);

        this.loader.hide();
      },
      error: (err) => {
        console.error('Login failed', err);
        if (err.status === 0) {
          this.errorMessage.set('Unable to reach server. Please try again.');
        } else if (err.status === 401) {
          this.errorMessage.set('Incorrect email or password.');
        } else if (err.status === 400) {
          this.errorMessage.set('Invalid request. Please check your input.');
        } else {
          this.errorMessage.set('Unexpected error. Please try again later.');
        }

        this.loader.hide();
      },
    });
  }
}
