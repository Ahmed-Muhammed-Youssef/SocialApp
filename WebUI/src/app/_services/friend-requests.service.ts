import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class FriendRequestsService {

  constructor(private http: HttpClient) { }
  baseUrl = environment.apiUrl;
  sendFriendRequest(username: string): Observable<boolean> {
    return this.http.post<boolean>(this.baseUrl + 'friendrequests/send/' + username, {}).pipe(map(r => {
      // this.usersChache = new Map();
      return r;
    }));
  }
  getSentFriendRequests(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'friendrequests/sent');
  }
  isFriendRequested(username: string): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'friendrequests/isSent/' + username);
  }
  getFriendRequests(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'friendrequests');
  }
  cancelFriendRequest(username: string): Observable<any>{
    return this.http.post<any>(this.baseUrl + 'friendrequests/cancel/' + username, {});
  }
}
