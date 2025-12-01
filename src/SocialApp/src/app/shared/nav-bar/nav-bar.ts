import { Component, inject } from '@angular/core';
import { AuthService } from '../../auth/services/auth';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-nav-bar',
  imports: [MatButtonModule, RouterModule],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css',
})
export class NavBar {
  authService = inject(AuthService);
  router = inject(Router);
  logOut(){
    this.authService.clear();
    this.router.navigate(['/login']);
  }
}
