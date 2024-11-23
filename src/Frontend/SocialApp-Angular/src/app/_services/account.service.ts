import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable, ReplaySubject } from 'rxjs';
import { LoginModel, LoginResponse, RegisterModel } from '../_models/AccountModels';
import { PresenceService } from './presence.service';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = "https://localhost:5001";
  constructor(private http: HttpClient, private presenceService: PresenceService) { }
  private currentUserSource = new ReplaySubject<LoginResponse | null>(1);
  public currentUser$: Observable<LoginResponse | null> = this.currentUserSource.asObservable();

  login(loginCred: LoginModel): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.baseUrl + 'account/login', loginCred).pipe<LoginResponse>(
      map<LoginResponse, LoginResponse>((response: LoginResponse) => {
        const loginResponse: LoginResponse = response;
        if (loginResponse) {
          this.setCurrentUser(loginResponse);
          this.presenceService.createHubConnection(response);
        }
        return response;
      })
    );
  }
  updateCachedProfilePicture(pictureUrl: string) {
    let user = localStorage.getItem('user') as LoginResponse | null;
    if(user)
    {
      user.userData.profilePictureUrl = pictureUrl;
      localStorage.removeItem('user');
      this.setCurrentUser(user);
    }
  }

  setCurrentUser(loginResponse: LoginResponse): void {
    loginResponse.userData.roles = [];
    const roles = this.getDecodedToken(loginResponse.token).role;
    Array.isArray(roles) ? loginResponse.userData.roles = roles : loginResponse.userData.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(loginResponse));
    this.currentUserSource.next(loginResponse);
  }

  logout(): void {
    localStorage.removeItem('user');
    this.presenceService.stopHubConnection();
    this.currentUserSource.next(null);
  }
  register(model: RegisterModel): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.baseUrl + 'account/register', model).pipe<LoginResponse>(
      map<LoginResponse, LoginResponse>((response: LoginResponse) => {
        if (response) {
          this.setCurrentUser(response);
          this.presenceService.createHubConnection(response);
        }
        return response;
      })
    );
  }
  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]))
  }
}
