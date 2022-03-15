import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Message } from '../_models/message';
import { PaginatedResult, Pagination } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  private paginationInfo : Pagination = {
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0
};
  constructor(private http: HttpClient) { }
  sendMessage(username: string, content: string){
    return this.http.post('api/messages', {"recipientUsername": username, "content": content});
  }

 getMessages(pageNumber : number = 1, itemsPerPage: number = 2, mode: string = 'unread') : Observable<PaginatedResult<Message[]>>{
  let httpParams: HttpParams = new HttpParams().set('pageNumber', pageNumber)
  .set('itemsPerPage', itemsPerPage).set('mode', mode);
  let paginatedResult : PaginatedResult<Message[]> = {result: [], pagination: this.paginationInfo};
  return this.http.get<Message[]>('/api/messages', {observe:'response', params: httpParams})
  .pipe(
    map(response => {
      if(response?.body){
        paginatedResult.result = response.body;
      }
      if(response.headers.get('Pagination')){
        paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') as string) as Pagination;
      }
      this.paginationInfo = paginatedResult.pagination;
      return paginatedResult as  PaginatedResult<Message[]>;
  }));
 }
 loadChat(username:string){
  return this.http.get<Message[]>('/api/messages/inbox/' + username);
 }
 deleteMessage(msgId: number){
   return this.http.delete('/api/Messages/' + msgId);
 }

}
