import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedList } from '../../shared/models/paged-list';
import { ChatDto } from '../models/chat-dto';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ChatsService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/chat`;

  getChats(pageNumber: number, itemsPerPage: number): Observable<PagedList<ChatDto>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('itemsPerPage', itemsPerPage);

    return this.http.get<PagedList<ChatDto>>(this.baseUrl, { params });
  }

}
