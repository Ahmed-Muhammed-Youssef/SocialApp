import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PictureService {
  baseUrl = environment.apiUrl + 'users/photo/upload';    
  constructor(private http:HttpClient) { }

  public uploadImage(image: File): Observable<Response> {
    const formData = new FormData();

    formData.append('image', image);
    return this.http.post<Response>(this.baseUrl, formData);
  }
}
