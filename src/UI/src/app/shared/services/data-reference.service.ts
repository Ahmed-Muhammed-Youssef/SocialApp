import { inject, Injectable } from '@angular/core';
import { CityDTO } from '../models/city-dto';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class DataReferenceService {
  private readonly baseUrl = `${environment.apiUrl}/referencedata/cities`;
  private httpClient = inject(HttpClient);

  getCities(): Observable<CityDTO[]> {
    return this.httpClient.get<CityDTO[]>(`${this.baseUrl}`);
  }
}
