import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginResponse } from '../_models/AccountModels';
import { Message } from '../_models/message';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection | null = null;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();
  constructor(private http: HttpClient) { }
  async sendMessage(id: number, content: string){
    return this.hubConnection?.invoke("SendMessages", {"recipientId": id, "content": content})
    .catch(e => {});
  }
  createHubConnection(user: LoginResponse, otherUserId: number){
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl + 'message?userId=' + otherUserId, 
    {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build();
    this.hubConnection.start().catch(e => {});
    this.hubConnection.on("ReceiveMessages", r => this.messageThreadSource.next(r));
    this.hubConnection.on("NewMessage", m => {
      this.messageThread$.pipe(take(1)).subscribe(msgs => {
        this.messageThreadSource.next([...msgs, m]);
      });
    });
  }
  stopHubConnection(){
    if(this.hubConnection){
      this.hubConnection.stop();
    } 
  }
 deleteMessage(msgId: number){
   return this.http.delete(this.baseUrl +'Messages/' + msgId);
 }
}

