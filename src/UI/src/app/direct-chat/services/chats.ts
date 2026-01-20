import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedList } from '../../shared/models/paged-list';
import { ChatDto } from '../models/chat-dto';

@Injectable({
  providedIn: 'root',
})
export class ChatsService {
  private http = inject(HttpClient);
  private readonly baseUrl = 'https://localhost:5001/api/chat';

  getChats(pageNumber: number, itemsPerPage: number): Observable<PagedList<ChatDto>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('itemsPerPage', itemsPerPage);

    return this.http.get<PagedList<ChatDto>>(this.baseUrl, { params });
  }

}
