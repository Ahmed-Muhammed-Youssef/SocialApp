import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';

export const notAuthenticatedGuard: CanActivateFn = (route, state) => {
 let authService = inject(AuthService);
  const router = inject(Router);
  return !authService.isLoggedIn() || router.createUrlTree(['/newsfeed']);
};
