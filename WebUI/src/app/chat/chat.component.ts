import { AfterViewChecked, ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { LoginResponse } from '../_models/AccountModels';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';
import { MessageService } from '../_services/message.service';
import { PresenceService } from '../_services/presence.service';
import { TimeFormatterService } from '../_services/activityTimeForamtter.service';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy, AfterViewChecked {
  @Input() currentFriend: User | undefined;
  @ViewChild('sendForm') sendForm?: NgForm;
  currentAccount: LoginResponse | null = null;
  newMessage: string = "";
  constructor(public messageService: MessageService, accountService: AccountService,
     public presenceService: PresenceService, public timeFormatterService:TimeFormatterService) {
    accountService.currentUser$.pipe(take(1)).subscribe(
      r => {
        this.currentAccount = r;
      }
    );
  }
  ngAfterViewChecked(): void {
    this.scrollChatHistoryDown();
  }
  ngOnInit(): void {
    this.loadChat();
  }
  sendMessage() {
    if (this.currentFriend) {
      this.messageService.sendMessage(this.currentFriend?.username, this.newMessage).then(
        () => { }
      );
      this.newMessage = '';
    
    }
  }
  loadChat() {
    if (this.currentFriend) {
      this.messageService.stopHubConnection();
      if (this.currentAccount) {
        this.messageService.createHubConnection(this.currentAccount, this.currentFriend.id);
      }
    }
  }
  scrollChatHistoryDown(){
    var nestedElement = document.getElementById("chat-list-scroll");
    if(nestedElement)
    {
      nestedElement.scrollTop = nestedElement?.scrollHeight;
    }
  }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
  
}
