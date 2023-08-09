import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { LoginResponse } from '../_models/AccountModels';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/User';
import { MessageService } from '../_services/message.service';
import { PresenceService } from '../_services/presence.service';
import { UserService } from '../_services/user.service';
import { BreakpointObserver } from '@angular/cdk/layout';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {
  mode = 'unread';
  matchesPagination?: Pagination;
  matches: User[] | null = null;
  matchPageNumber = 1;
  matchesPerPage = 10;
  currentAccount: LoginResponse | null = null;
  newMessage: string = "";
  isMobilePhone: boolean = false;

  constructor(public messageService: MessageService, private userService: UserService,
  public presenceService: PresenceService, private changeDetectorRef: ChangeDetectorRef, private breakpointObserver: BreakpointObserver) {
    breakpointObserver.observe(["(max-width: 750px)"])
      .subscribe(
        result => {
          this.isMobilePhone = false;
          if (result.matches) {
            this.isMobilePhone = true;
          }
        }
      ); }

  ngOnInit(): void {
    this.loadMatches();
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
          this.changeDetectorRef.detectChanges();
        }
      }
    );
  }
  loadChat(user: User){
    // not implemented yet
  }
  public getLoacaleDateTime(d: Date) : Date{
    var localDate  = new Date(d.toString() + 'Z');
    return localDate;
  }
  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
