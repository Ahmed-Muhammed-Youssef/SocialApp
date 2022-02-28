import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { Photo, UpdateUser, User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }
  public users: User[] = [];

  public getAllUsers(): Observable<User[]> {
    if (this.users.length > 0) {
      return of(this.users);
    }
    return this.http.get<User[]>('/api/users/all').pipe(map(
      users => {
        if(users){
          this.users = users;
        }
        return users;
      }
    ));
  }
  public getUserByUsername(username: string): Observable<User> {
    const user = this.users.find(u => u.username === username);
    if (user) {
      return of(user);
    }
    return this.http.get<User>('/api/users/info/username/' + username);
  }
  public reorderPhotos(photos: Photo[]){
    return this.http.put<Photo[]>('/api/users/photos/reorder', photos);
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
    return this.http.put<UpdateUser>('/api/users/update', userTosend).pipe(map(
      () => {
        const index = this.users.indexOf(user);
        this.users[index] = user;
        return user;
      }
    ));
  }
}
