import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, map, Observable, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginResponse } from '../_models/AccountModels';
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
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubUrl;
  private hubConnection: HubConnection | null = null;
  private messageThreadSource = new BehaviorSubject<Message[]>([]);
  messageThread$ = this.messageThreadSource.asObservable();
  constructor(private http: HttpClient) { }
  async sendMessage(username: string, content: string){
    return this.hubConnection?.invoke("SendMessages", {"recipientUsername": username, "content": content})
    .catch(e => console.log(e));
  }
  createHubConnection(user: LoginResponse, otherUserId: number){
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl + 'message?userId=' + otherUserId, 
    {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build();
    this.hubConnection.start().catch(e => console.log(e));
    this.hubConnection.on("ReceiveMessages", r => this.messageThreadSource.next(r));
    this.hubConnection.on("NewMessage", m => {
      console.log(m);
      this.messageThread$.pipe(take(1)).subscribe(msgs => {
        this.messageThreadSource.next([...msgs, m]);
      });
    });
  }
  stopHubConnection(){
    if(this.hubConnection){
      this.hubConnection.stop();
    } 
  }
 getMessages(pageNumber : number = 1, itemsPerPage: number = 2, mode: string = 'unread') : Observable<PaginatedResult<Message[]>>{
  let httpParams: HttpParams = new HttpParams().set('pageNumber', pageNumber)
  .set('itemsPerPage', itemsPerPage).set('mode', mode);
  let paginatedResult : PaginatedResult<Message[]> = {result: [], pagination: this.paginationInfo};
  return this.http.get<Message[]>(this.baseUrl + 'messages', {observe:'response', params: httpParams})
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
  return this.http.get<Message[]>(this.baseUrl +'messages/inbox/' + username);
 }
 deleteMessage(msgId: number){
   return this.http.delete(this.baseUrl +'Messages/' + msgId);
 }

}

