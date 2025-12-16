import { AfterViewInit, Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../services/auth';

import { Router, RouterModule } from '@angular/router';
import { LoaderService } from '../../shared/services/loader';
import { delay } from 'rxjs';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { GoogleCredentialResponse, GooglePromptNotification } from '../models/google-identity';
@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterModule, MatFormFieldModule, MatInputModule, MatIconModule, MatButtonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login implements AfterViewInit {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  private loader = inject(LoaderService);

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  errorMessage = signal<string | null>(null);

  async ngAfterViewInit(): Promise<void> {
    await this.loadGoogleIdentityScript();
    this.initializeGoogleSignIn();
    this.renderGoogleButton();
  }

  private initializeGoogleSignIn(): void {
    window.google.accounts.id.initialize({
      client_id: '640969805450-3ailvii3einnh7ruuncpo8k20kbk9slr.apps.googleusercontent.com',
      callback: this.handleCredentialResponse.bind(this),
      cancel_on_tap_outside: true,
    });
  }
  onSubmit() {
    if (this.loginForm.invalid) return;
    this.errorMessage.set(null);

    this.loader.show();

    this.auth.login(this.loginForm.value)
      .pipe(delay(500))
      .subscribe({
        next: (res) => {
          this.errorMessage.set(null);
          this.router.navigate(['/newsfeed']);

          this.loader.hide();
        },
        error: (err) => {
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

  hide = signal(true);
  clickEvent(event: MouseEvent) {
    this.hide.set(!this.hide());
    event.stopPropagation();
  }

   private renderGoogleButton(): void {
    const container = document.getElementById('googleLoginButton');
    if (!container) {
      throw new Error('Google button container not found');
    }

    window.google.accounts.id.renderButton(container, {
      theme: 'outline',
      size: 'large',
      text: 'continue_with',
      shape: 'pill',
    });
  }

  triggerGoogleOneTap(): void {
    window.google.accounts.id.prompt(
      (notification: GooglePromptNotification) => {
        if (notification.isNotDisplayed()) {
          console.warn(
            'One Tap not displayed:',
            notification.getNotDisplayedReason()
          );
        }
      }
    );
  }

 private handleCredentialResponse(
    response: GoogleCredentialResponse
  ): void {
    console.log('Google Credential Response:', response);
    this.auth.googleLogin(response.credential).subscribe({
      next: () => {
        console.log('Google login successful');
        // navigate or handle success
      },
      error: () => {
        console.error('Google login failed');
        // handle error
      },
    });
  }

  loadGoogleIdentityScript(): Promise<void> {
  return new Promise((resolve, reject) => {
    if (window.google?.accounts?.id) {
      resolve();
      return;
    }

    const script = document.createElement('script');
    script.src = 'https://accounts.google.com/gsi/client';
    script.async = true;
    script.defer = true;

    script.onload = () => resolve();
    script.onerror = () => reject('Failed to load Google Identity script');

    document.head.appendChild(script);
  });
}

}
