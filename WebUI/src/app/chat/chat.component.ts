import { ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { LoginResponse } from '../_models/AccountModels';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';
import { MessageService } from '../_services/message.service';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit,OnDestroy {
  mode = 'unread';
  @Input () currentMatch?: User;
  @ViewChild('sendForm') sendForm?: NgForm; 
  matchesPagination?: Pagination;
  matches: User[] | null = null;
  matchPageNumber = 1;
  matchesPerPage = 10;
  currentAccount: LoginResponse | null = null;
  newMessage: string = "";
  constructor(public messageService: MessageService, private userService: UserService,
    accountService: AccountService, public presenceService: PresenceService) { 
    
      accountService.currentUser$.pipe(take(1)).subscribe(
      r => {
        this.currentAccount = r;
      }
    );
  }
  public getLoacaleDateTime(d: Date) : Date{
    var localDate  = new Date(d.toString() + 'Z');
    return localDate;
  }
  ngOnInit(): void {
    this.loadMatches();
    if(history.state?.username){
      this.currentMatch = history.state as User;
    }
    if(this.currentMatch){
      this.loadChat(this.currentMatch);
    }
  }

  matchPageChanged(e: any){
    if(e && (e.pageIndex + 1) != this.matchPageNumber){
      this.matchPageNumber = e.pageIndex + 1;
      this.loadMatches();
    }
  }
  loadMatches(){
    this.userService.getMatches(this.matchPageNumber, this.matchesPerPage).subscribe(
      r => {
        if(r){
          this.matches = r.result;
          this.matchesPagination = r.pagination;
        }
      }
    );
  }
  sendMessage(){
    if(this.currentMatch){
      this.messageService.sendMessage(this.currentMatch?.username, this.newMessage).then(
        () => {}
      );
      this.newMessage = '';     
    }
  }
  loadChat(user: User){
    this.messageService.stopHubConnection();
    if(this.currentAccount){
      this.messageService.createHubConnection(this.currentAccount, user.id);
      this.currentMatch = user;
    }
  }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

}
