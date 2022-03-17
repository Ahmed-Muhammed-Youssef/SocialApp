import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/User';
import { MessageService } from '../_services/message.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[] = [];
  mode = 'unread';
  @Input () currentMatch?: User;
  matchesPagination?: Pagination;
  matches: User[] = [];
  matchPageNumber = 1;
  matchesPerPage = 10;

  newMessage: string = "";
  constructor(private messageService: MessageService, private userService: UserService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    console.log(history.state);
    this.loadMatches();
    if(history.state?.username){
      this.currentMatch = history.state as User;
    }
    if(this.currentMatch){
      this.loadChat(this.currentMatch);
    }
}

  matchPageChanged(e: any){
    if(e && e.page != this.matchPageNumber){
      this.matchPageNumber = e.page;
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
      this.messageService.sendMessage(this.currentMatch?.username, this.newMessage).subscribe();
      this.newMessage = "";
      this.loadChat(this.currentMatch);
    }
  }
  loadChat(user: User){
    this.messageService.loadChat(user.username).subscribe(r => {
      this.messages = r.reverse();
      this.currentMatch = user;
    });
  }
}
