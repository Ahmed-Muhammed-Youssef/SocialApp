import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UpdateUser, User } from '../_models/User';

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
  updateUser(user: User): Observable<UpdateUser> {
    const userTosend: UpdateUser  = {
      firstName: user.firstName,
      lastName: user.lastName,
      interest: user.interest,
      bio: user.bio,
      city: user.city,
      country: user.country
    };
    return this.http.put<UpdateUser>('/api/users/update', userTosend);
  }
}
