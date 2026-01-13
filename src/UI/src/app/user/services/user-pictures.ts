import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PictureDTO } from '../../auth/models/user-dto';

@Injectable({
  providedIn: 'root',
})
export class UserPicturesService {
  private readonly baseUrl = 'https://localhost:5001/api/users';
  private httpClient = inject(HttpClient);

  uploadUserPicture(file: File) : Observable<number> {
    const formData = new FormData();
    formData.append('file', file);
    return this.httpClient.post<number>(`${this.baseUrl}/user-pictures`, formData);
  }

  setProfilePicture(pictureId: number) : Observable<void> {
    return this.httpClient.post<void>(`${this.baseUrl}/set-profile-picture/${pictureId}`, null);
  }
  
  GetUserPictures() : Observable<PictureDTO[]> {
    return this.httpClient.get<PictureDTO[]>(`${this.baseUrl}/user-pictures`);
  }
}
