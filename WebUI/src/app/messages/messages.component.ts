import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { LoginResponse } from '../_models/AccountModels';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';
import { MessageService } from '../_services/message.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit,OnDestroy {
  mode = 'unread';
  @Input () currentMatch?: User;
  @ViewChild('sendForm') sendForm?: NgForm; 
  matchesPagination?: Pagination;
  matches: User[] = [];
  matchPageNumber = 1;
  matchesPerPage = 10;
  currentAccount: LoginResponse | null = null;
  newMessage: string = "";
  constructor(public messageService: MessageService, private userService: UserService,
    private accountService: AccountService) { 
    
      accountService.currentUser$.pipe(take(1)).subscribe(
      r => {
        this.currentAccount = r;
      }
    );
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
      this.sendForm?.reset();      
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
