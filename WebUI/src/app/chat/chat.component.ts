import { AfterViewChecked, ChangeDetectionStrategy, Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { take } from 'rxjs';
import { LoginResponse } from '../_models/AccountModels';
import { User } from '../_models/User';
import { AccountService } from '../_services/account.service';
import { MessageService } from '../_services/message.service';
import { PresenceService } from '../_services/presence.service';

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
  constructor(public messageService: MessageService, accountService: AccountService, public presenceService: PresenceService) {
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
  private getLoacaleDateTime(d: Date): Date {
    var localDate = new Date(d.toString() + 'Z');
    return localDate;
  }
  public getDateTimeAgo(date: Date) {
    var now = new Date();
    date = this.getLoacaleDateTime(date);
    var yearDiff = now.getFullYear() - date.getFullYear();
    var monthDiff = now.getMonth()- date.getMonth();
    var dayDiff = now.getDate()- date.getDate();
    var hourDiff = now.getHours() - date.getHours();
    var minuteDiff = now.getMinutes() - date.getMinutes();
    if(yearDiff > 0)
    {
      return yearDiff + ' year'+ (yearDiff> 1? 's':'') + ' ago';
    }
    else 
    if (monthDiff > 0){
      return monthDiff + ' month'+ (monthDiff> 1? 's':'') + ' ago';
    }
    else if (dayDiff > 0){
      return dayDiff + ' day'+ (dayDiff> 1? 's':'') + ' ago';
    }
    else if (hourDiff > 0){
      return hourDiff + ' hour'+ (hourDiff> 1? 's':'') + ' ago';
    }
    else if (minuteDiff > 0){
      return minuteDiff + ' minute'+ (minuteDiff> 1? 's':'') + ' ago';
    }
    else {
      return 'online';
    }
  }
}
