import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserDTO } from '../../auth/models/user-dto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private readonly baseUrl = 'https://localhost:5001/api/users';
  private httpClient = inject(HttpClient);

  
  getUserById(userId : number) : Observable<UserDTO>
  {
    return this.httpClient.get<UserDTO>(`${this.baseUrl}/${userId}`)
  }
}
