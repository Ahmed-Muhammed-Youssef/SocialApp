import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LoginRequest } from '../models/login-request';
import { Observable, tap } from 'rxjs';
import { AuthResponse } from '../models/auth-response';
import { UserDTO } from '../models/user-dto';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http: HttpClient = inject(HttpClient);
  private token: string | null = null;
  private userData: UserDTO | null = null;
  private readonly baseUrl = 'https://localhost:5001/api/auth';

  constructor() {
    this.token = sessionStorage.getItem('access_token');
    try {
      const data = sessionStorage.getItem('user_data');
      this.userData = data ? JSON.parse(data) : null;
    } catch {
      this.userData = null;
      sessionStorage.removeItem('user_data');
    }
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.baseUrl}/login`, request)
      .pipe(
        tap(res => {
          this.setToken(res.token);
          this.setUserData(res.userData);
        })
      );
  }

  getToken(): string | null {
    return this.token;
  }

  getUserData(): UserDTO | null {
    return this.userData;
  }

  private setUserData(userData: UserDTO) {
    this.userData = userData;
    sessionStorage.setItem('user_data', JSON.stringify(userData));
  }

  private setToken(t: string) {
    this.token = t;
    sessionStorage.setItem('access_token', this.token);
  }

  clear() {
    this.token = null;
    this.userData = null;

    sessionStorage.removeItem('access_token');
    sessionStorage.removeItem('user_data');
  }

  isLoggedIn(): boolean {
    return !!sessionStorage.getItem('access_token');
  }
}
