import { Injectable } from '@angular/core';
import { LoginResponse } from '../_models/AccountModels';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  baseUrl = "https://localhost:5001";
  onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();
  private hubConnection: HubConnection | null = null;
  constructor(private toastr: ToastrService, private router: Router) { }
  createHubConnection(user: LoginResponse){
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.baseUrl + 'presence', 
    {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build();
    this.hubConnection
    .start()
    .catch(e => {});
    this.hubConnection.on("UserIsOnline", username => {
      this.onlineUsers$.pipe(take(1))
      .subscribe(usernames => 
        {
          this.onlineUsersSource.next([...usernames, username]);
        });
    });
    this.hubConnection.on("UserIsOffline", username => {
      this.onlineUsers$.pipe(take(1))
      .subscribe(usernames => 
        {
          this.onlineUsersSource.next([...usernames.filter(u => u != username)]);
        });
    });
    this.hubConnection.on("GetOnlineUsers",
     (usernames) =>
      {
        this.onlineUsersSource.next(usernames);
      });
      this.hubConnection.on("NewMessage", 
      (userMsg) => {
        this.toastr.info(userMsg.senderDTO.firstName + ": " + userMsg.msgDTO.content)
        .onTap
        .pipe(take(1))
        .subscribe(() => {
          this.router.navigate(["/messages",  {username: userMsg.senderDTO.username}]);
        });
      });
  }
  stopHubConnection(){
    this.hubConnection?.stop().catch(e => {});
  }
}
