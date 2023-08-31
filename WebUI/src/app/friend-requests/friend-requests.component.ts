import { Component, OnInit } from '@angular/core';
import { FriendRequestsService } from '../_services/friend-requests.service';
import { User } from '../_models/User';
import { BreakpointObserver } from '@angular/cdk/layout';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-friend-requests',
  templateUrl: './friend-requests.component.html',
  styleUrls: ['./friend-requests.component.css']
})
export class FriendRequestsComponent implements OnInit {
  friendRequests: User[] = [];
  isMobilePhone = false;
  constructor(private friendRequestsService: FriendRequestsService,
    breakpointObserver: BreakpointObserver, private toastr: ToastrService) {
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
    this.loadFriendRequests();
  }
  loadFriendRequests() {
    this.friendRequestsService.getFriendRequests().subscribe(
      result => {
        if (result) this.friendRequests = result;
        console.log(result);
      });
  }
  sendFriendRequest(user: User) {
    this.friendRequestsService.sendFriendRequest(user.username).subscribe(
      r => {
        if (r == true) {
          this.toastr.success(user.firstName + " is added to your friends list successfully.")
        }
      }
    );
  }
}
