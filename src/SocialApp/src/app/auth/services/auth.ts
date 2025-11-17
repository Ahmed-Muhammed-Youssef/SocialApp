import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LoginRequest } from '../models/LoginRequest';
import { Observable } from 'rxjs';
import { AuthResponse } from '../models/AuthResponse';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private http: HttpClient = inject(HttpClient);

  private readonly baseUrl = 'https://localhost:5001/api/auth';

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, request);
  }
}
