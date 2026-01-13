import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../auth/services/auth';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-nav-bar',
  imports: [MatButtonModule, RouterModule, MatMenuModule, MatIconModule],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css',
})
export class NavBar {
  authService = inject(AuthService);
  router = inject(Router);

  // These would normally come from a ChatService
  chats = signal([
    { id: 1, name: 'Alice', avatar: 'imgs/default-user.png' },
    { id: 2, name: 'Bob', avatar: 'imgs/default-user.png' }
  ]);

  unreadCount = signal(2);

  userId = () => this.authService.getUserData()?.id ?? 0;
  avatar = () => this.authService.getUserData()?.profilePictureUrl ?? 'imgs/default-user.png';
  username = () => this.authService.getUserData()?.firstName ?? 'User';

  logOut(){
    this.authService.clear();
    this.router.navigate(['/login']);
  }

  openChat(id: number) {
    this.router.navigate(['/chat', id]);
  }
}
