import { inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { ChatMessage } from '../../direct-chat/models/chat-message';
import { AuthService } from '../../auth/services/auth';

@Injectable({
  providedIn: 'root',
})
export class ChatHubService {
  private hubConnection!: signalR.HubConnection;

  private messagesSubject = new BehaviorSubject<ChatMessage[]>([]);
  public messages$ = this.messagesSubject.asObservable();
  private authService = inject(AuthService);
  private readonly baseUrl = 'https://localhost:5001/hubs/' + 'message';

  async startConnection(otherUserId: number): Promise<void> {
    let token = this.authService.getToken();
    if (!token) {
      return Promise.reject('No auth token available');
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.baseUrl}?userId=${otherUserId}`, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.registerHandlers();

    try {
      return await this.hubConnection
        .start();
    } catch (err) {
      return console.error('SignalR connection error:', err);
    }
  }

  private registerHandlers(): void {
    // Initial load of messages
    this.hubConnection.on('ReceiveMessages', (messages: ChatMessage[]) => {
      this.messagesSubject.next(messages);
    });

    // Incoming message
    this.hubConnection.on('NewMessage', (message: ChatMessage) => {
      const current = this.messagesSubject.value;
      this.messagesSubject.next([...current, message]);
    });
  }

  sendMessage(recipientId: number, content: string) {
    return this.hubConnection.invoke('SendMessage', {
      recipientId,
      content
    });
  }

  stop(): Promise<void> {
    return this.hubConnection?.stop();
  }
}
