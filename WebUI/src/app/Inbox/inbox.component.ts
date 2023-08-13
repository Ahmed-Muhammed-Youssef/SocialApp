import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { LoginResponse } from '../_models/AccountModels';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/User';
import { MessageService } from '../_services/message.service';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';
import { BreakpointObserver } from '@angular/cdk/layout';
import { ChatComponent } from '../chat/chat.component';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-inbox',
  templateUrl: './inbox.component.html',
  styleUrls: ['./inbox.component.css']
})
export class InboxComponent implements OnInit {
  @ViewChild(ChatComponent) chatComponent!: ChatComponent;
  currentUserSelection: User | undefined;
  friendsPagination?: Pagination;
  friends: User[] | null = null;
  friendsPageNumber = 1;
  friendsPerPage = 3;
  currentAccount: LoginResponse | null = null;
  newMessage: string = "";
  isMobilePhone: boolean = false;

  constructor(public messageService: MessageService, private userService: UserService,
    public presenceService: PresenceService, private changeDetectorRef: ChangeDetectorRef,
    private breakpointObserver: BreakpointObserver, private route: ActivatedRoute) {
    // Get username from url params
    var username = this.route.snapshot.paramMap.get('username');
    if (username) {
      this.userService.getUserByUsername(username).subscribe(
        response => {
          if (response) {
            this.currentUserSelection = response;
          }
        });
    }

    // Set break point 
    breakpointObserver.observe(["(max-width: 750px)"])
      .subscribe(
        result => {
          this.isMobilePhone = false;
          if (result.matches) {
            this.isMobilePhone = true;
          }
        }
      );
  }

  ngOnInit(): void {
    this.loadFriends();
  }
  friendsPageChanged(e: any) {
    if (e && (e.pageIndex + 1) != this.friendsPageNumber) {
      this.friendsPageNumber = e.pageIndex + 1;
      this.loadFriends();
    }
  }
  loadFriends() {
    this.userService.getMatches(this.friendsPageNumber, this.friendsPerPage).subscribe(
      r => {
        if (r) {
          this.friends = r.result;
          this.friendsPagination = r.pagination;
          // this.changeDetectorRef.detectChanges();
        }
      }
    );
  }
  loadChat(user: User) {
    this.currentUserSelection = user;
    this.changeDetectorRef.detectChanges();
    this.chatComponent.loadChat();
  }
  public getLoacaleDateTime(d: Date): Date {
    var localDate = new Date(d.toString() + 'Z');
    return localDate;
  }
  public getDateTimeAgo(date: Date) {
    var now = new Date();
    date = this.getLoacaleDateTime(date);
    var yearDiff = now.getFullYear() - date.getFullYear();
    var monthDiff = now.getMonth() - date.getMonth();
    var dayDiff = now.getDate() - date.getDate();
    var hourDiff = now.getHours() - date.getHours();
    var minuteDiff = now.getMinutes() - date.getMinutes();
    if (yearDiff > 0) {
      return yearDiff + ' year' + (yearDiff > 1 ? 's' : '') + ' ago';
    }
    else
      if (monthDiff > 0) {
        return monthDiff + ' month' + (monthDiff > 1 ? 's' : '') + ' ago';
      }
      else if (dayDiff > 0) {
        return dayDiff + ' day' + (dayDiff > 1 ? 's' : '') + ' ago';
      }
      else if (hourDiff > 0) {
        return hourDiff + ' hour' + (hourDiff > 1 ? 's' : '') + ' ago';
      }
      else if (minuteDiff > 0) {
        return minuteDiff + ' minute' + (minuteDiff > 1 ? 's' : '') + ' ago';
      }
      else {
        return 'online';
      }
  }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
