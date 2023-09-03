import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Picture, User } from '../../_models/User';
import { UserService } from '../../_services/user.service';

import { ToastrService } from 'ngx-toastr';
import { PresenceService } from 'src/app/_services/presence.service';
import { BreakpointObserver } from '@angular/cdk/layout';
import { TimeFormatterService } from 'src/app/_services/activityTimeForamtter.service';
import { FriendRequestsService } from 'src/app/_services/friend-requests.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
  isMobilePhone : boolean = false;
  profilePicture : Picture | undefined;
  user: User = {
    id: 0,
    username: '',
    firstName: '',
    lastName: '',
    sex: '',
    interest: '',
    age: 0,
    created: new Date(),
    lastActive: new Date(),
    bio: '',
    city: '',
    country: '',
    pictures: [],
    roles: []
  };
  isFriendRequested : boolean = true;
  isFriend: boolean = false;
  
  constructor(private friendRequestsService: FriendRequestsService,private userService: UserService ,private route: ActivatedRoute,
     private toastr: ToastrService, public presenceService: PresenceService,
      private breakpointObserver: BreakpointObserver, public timeFormatterService:TimeFormatterService) {
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
  sendFriendRequest(){
    this.userService.deleteCachedValues();
    this.friendRequestsService.sendFriendRequest(this.user.username).subscribe(
      r => {
          this.isFriendRequested = true;
          this.toastr.success('You have sent frined request to ' + this.user.firstName);
          if(r == true){
            this.toastr.success("You have a new friend!");
          }
      }
    );
  }
  ngOnInit(): void {
    this.route.data.subscribe(
      data => {
        this.user = data.user;
        this.profilePicture = this.user.pictures[0];
        this.setImages();
        this.friendRequestsService.isFriendRequested(this.user.username).subscribe(r => {
          this.isFriendRequested = r;
        });
      this.userService.isFriend(this.user.username).subscribe(r => this.isFriend = r);
      }
    );
  }
  public formatInterest(interest: string): string {
    if (interest === 'f') {
      return 'Females'
    }
    else if (interest === 'm') {
      return 'Males'
    }
    else {
      return 'Both females and males'
    }
  }

  // need to be replaced
  public setImages() {
   
  }
}
