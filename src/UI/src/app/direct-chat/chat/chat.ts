import { afterRenderEffect, Component, DestroyRef, ElementRef, inject, signal, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChatHubService } from '../../shared/services/chat-hub.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { DatePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../auth/services/auth';
import { UsersService } from '../../user/services/users';
import { UserDTO } from '../../auth/models/user-dto';

@Component({
  selector: 'app-chat',
  imports: [MatIconModule, ReactiveFormsModule, MatInputModule, MatButtonModule, DatePipe, MatCardModule, FormsModule],
  templateUrl: './chat.html',
  styleUrl: './chat.css',
})
export class Chat {
  private readonly route = inject(ActivatedRoute);
  private readonly chatHub = inject(ChatHubService);
  private authService = inject(AuthService);
  private userService = inject(UsersService);

  private readonly destroyRef = inject(DestroyRef);
  @ViewChild('scrollContainer') private scrollContainer!: ElementRef;

  readonly userId = () => this.authService.getUserData()?.id ?? 0;
  otherUser = signal<UserDTO | null>(null);
  otherUserId = signal<number | null>(null);

  readonly messages = toSignal(this.chatHub.messages$, { initialValue: [] });
  readonly newMessage = signal('');
  
  newMessageContent = signal('');
  constructor() {
  this.route.paramMap.subscribe(params => {      
          this.otherUserId.set(Number(params.get('id')));

          const otherId = this.otherUserId();
          if (otherId != null) {
            this.chatHub.startConnection(otherId)
              .catch(err => console.error('SignalR connection failed:', err));
      
            this.userService.getUserById(otherId).subscribe({
              next: (res) => this.otherUser.set(res),
              error: (err) => console.error(err)
            });
          } else {
            console.error('Missing User ID. Cannot start chat.');
          }
        });

    this.destroyRef.onDestroy(() => {
      this.chatHub.stop();
      console.log('ChatHub connection stopped.');
    });

    afterRenderEffect(() => {
        this.scrollToBottom();
    });
  }

  sendMessage() {
    const content = this.newMessage().trim();
    if (!content) return;

    this.chatHub.sendMessage(this.otherUserId()!, content)
      .then(() => this.newMessage.set('')); // Clear input
  }

  private scrollToBottom() {
    if (this.scrollContainer?.nativeElement) {
      const el = this.scrollContainer.nativeElement;
      el.scrollTo({ top: el.scrollHeight, behavior: 'smooth' });
    }
  }
}
