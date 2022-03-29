import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
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
  constructor(private toastr: ToastrService) { }
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
      // do something
    });
    this.hubConnection.on("UserIsOffline", username => {
      // do something
    });
    this.hubConnection.on("GetOnlineUsers",
     (usernames) =>
      {
        this.onlineUsersSource.next(usernames);
      }); 
  }
  stopHubConnection(){
    this.hubConnection?.stop().catch(e => console.log(e));
  }
}
