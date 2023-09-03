import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PictureService {
  baseUrl = environment.apiUrl + 'pictures/upload';    
  constructor(private http:HttpClient) { }

  public uploadImage(image: File): Observable<any> {
    const formData = new FormData(); 
    formData.append("file", image, image.name);
    return this.http.post(this.baseUrl, formData, {reportProgress: true, observe: 'events'});
  }
  public deletePicture(pictureId: number) {
    return this.http.delete(this.baseUrl + 'pictures/' + String(pictureId));
  }
}
