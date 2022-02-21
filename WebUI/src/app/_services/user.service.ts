import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }
  getAllUsers():Observable<User[]> {
    return this.http.get<User[]>('/api/users/all');
  }
  getUserById(id: number): Observable<User> {
    return this.http.get<User>('/api/users/info/id/' + String(id));
  }
  getUserByUsername(username: string): Observable<User> {
    return this.http.get<User>('/api/users/info/username/' + username);
  }
}
