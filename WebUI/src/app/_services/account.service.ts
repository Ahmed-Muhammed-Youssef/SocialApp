import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, ReplaySubject } from 'rxjs';
import { LoginModel, LoginResponse, RegisterModel } from '../_models/AccountModels';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private http: HttpClient) { }
  private currentUserSource = new ReplaySubject<LoginResponse | null>(1);
  public currentUser$: Observable<LoginResponse | null> = this.currentUserSource.asObservable();

  login(loginCred: LoginModel): Observable<LoginResponse> {
    return this.http.post<LoginResponse>('/api/account/login', loginCred).pipe<LoginResponse>(
      map<LoginResponse, LoginResponse>((response: LoginResponse) => {
        const loginResponse: LoginResponse = response;
        if (loginResponse) {
          this. setCurrentUser(loginResponse);
        }
        return response;
      })
    );
  }

  setCurrentUser(loginResponse: LoginResponse): void {
    loginResponse.userData.roles = [];
    const roles = this.getDecodedToken(loginResponse.token).role;
    Array.isArray(roles)? loginResponse.userData.roles = roles: loginResponse.userData.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(loginResponse));
    this.currentUserSource.next(loginResponse);
  }

  logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
  register(model: RegisterModel): Observable<LoginResponse> {
    return this.http.post<LoginResponse>('/api/account/register', model).pipe<LoginResponse>(
      map<LoginResponse, LoginResponse>((response: LoginResponse) => {
        const loginResponse: LoginResponse = response;
        if (loginResponse) {
          this. setCurrentUser(loginResponse);
        }
        return response;
      })
    );
  }
  getDecodedToken(token: string){
    return JSON.parse(atob(token.split('.')[1]))
  }
}
