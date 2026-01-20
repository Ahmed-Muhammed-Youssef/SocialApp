import { Component, inject, signal, effect } from '@angular/core';
import { AuthService } from '../../auth/services/auth';
import { ChatsService } from '../../direct-chat/services/chats';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { ChatDto } from '../../direct-chat/models/chat-dto';

@Component({
  selector: 'app-nav-bar',
  imports: [MatButtonModule, RouterModule, MatMenuModule, MatIconModule, CommonModule],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css',
})
export class NavBar {
  authService = inject(AuthService);
  chatsService = inject(ChatsService);
  router = inject(Router);

  chats = signal<ChatDto[]>([]);
  isLoadingChats = signal(false);
  chatsError = signal<string | null>(null);
  unreadCount = signal(0);

  constructor() {
    // Load chats when component initializes
    this.loadChats();
  }

  private loadChats(): void {
    this.isLoadingChats.set(true);
    this.chatsError.set(null);

    this.chatsService.getChats(1, 5).subscribe({
      next: (response) => {
        this.chats.set(response.items);
        this.isLoadingChats.set(false);
      },
      error: (error) => {
        console.error('Failed to load chats:', error);
        this.chatsError.set('Failed to load chats');
        this.isLoadingChats.set(false);
      }
    });
  }

  userId = () => this.authService.getUserData()?.id ?? 0;
  avatar = () => this.authService.getUserData()?.profilePictureUrl ?? 'imgs/default-user.png';
  username = () => this.authService.getUserData()?.firstName ?? 'User';

  logOut(){
    this.authService.clear();
    this.router.navigate(['/login']);
  }

  openChat(userId: number) {
    this.router.navigate(['/chat', userId]);
  }

  getChatName(chat: ChatDto): string {
    return `${chat.userFirstName} ${chat.userLastName}`;
  }

  getChatAvatar(chat: ChatDto): string {
    // TODO: Get avatar from chat user profile picture
    return 'imgs/default-user.png';
  }
}
