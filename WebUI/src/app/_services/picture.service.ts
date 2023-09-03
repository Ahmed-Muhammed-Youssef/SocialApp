import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PictureService {
  baseUrl = environment.apiUrl;    
  constructor(private http:HttpClient) { }

  public uploadImage(image: File): Observable<any> {
    const formData = new FormData(); 
    formData.append("file", image, image.name);
    return this.http.post(this.baseUrl + 'pictures', formData, {reportProgress: true, observe: 'events'});
  }
  public deletePicture(pictureId: number) {
    return this.http.delete(this.baseUrl + 'pictures/' + String(pictureId));
  }
  public setProfilePicture(pictureId: number) {
    return this.http.post(this.baseUrl + 'pictures/profilepicture' + String(pictureId), {});
  }
}
