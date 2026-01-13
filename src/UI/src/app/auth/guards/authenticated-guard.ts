import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth';
import { inject } from '@angular/core';

export const authenticatedGuard: CanActivateFn = (route, state) => {
  let authService = inject(AuthService);
  const router = inject(Router);
  return authService.isLoggedIn() || router.createUrlTree(['']);
};
