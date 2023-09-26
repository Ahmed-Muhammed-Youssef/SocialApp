import { ChangeDetectorRef, Component, OnInit, ViewChild, numberAttribute } from '@angular/core';
import { LoginResponse } from '../_models/AccountModels';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/User';
import { MessageService } from '../_services/message.service';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';
import { BreakpointObserver } from '@angular/cdk/layout';
import { ChatComponent } from '../chat/chat.component';
import { ActivatedRoute } from '@angular/router';
import { TimeFormatterService } from '../_services/activityTimeForamtter.service';

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
    breakpointObserver: BreakpointObserver, private route: ActivatedRoute, 
    public timeFormatterService:TimeFormatterService) {
    // Get id from url params
    var id : number | null = this.route.snapshot.paramMap.get('id') as number | null;
    if (id) {
      this.userService.getUserById(id).subscribe(
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
    this.userService.getFriends(this.friendsPageNumber, this.friendsPerPage).subscribe(
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
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
