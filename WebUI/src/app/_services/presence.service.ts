import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginResponse } from '../_models/AccountModels';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  baseUrl = environment.hubUrl;
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
    .catch(e => {console.log(e)});
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
        console.log(userMsg);
        this.toastr.info(userMsg.senderDTO.firstName + ": " + userMsg.msgDTO.content)
        .onTap
        .pipe(take(1))
        .subscribe(() => {
          this.router.navigateByUrl("/messages", {state: userMsg.senderDTO} );
        });
      });
  }
  stopHubConnection(){
    this.hubConnection?.stop().catch(e => console.log(e));
  }
}
