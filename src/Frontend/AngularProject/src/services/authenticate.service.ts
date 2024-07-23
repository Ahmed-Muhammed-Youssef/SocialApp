import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class AuthenticateService {

  private apiURL = environment.apiUrl;

  constructor(private httpClient: HttpClient) { }

  getItems(): Observable<any> {
    const endpoint = `${this.apiURL}/`;
    return this.httpClient.get(endpoint);
  }
}
