import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PresenceService } from 'src/app/_services/presence.service';
import { UserService } from 'src/app/_services/user.service';
import { User } from '../../_models/User';
import { Router } from '@angular/router';
import { TimeFormatterService } from 'src/app/_services/activityTimeForamtter.service';
import { FriendRequestsService } from 'src/app/_services/friend-requests.service';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() isFriendRequested = false;
  @Output() friendRequested = new EventEmitter();
  public userProfilePicture: string | undefined;
  @Input() user: User = {
    id: 0,
    username: '',
    firstName: '',
    lastName: '',
    profilePictureUrl: '',
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
  constructor(private friendRequestsService: FriendRequestsService, private toastr: ToastrService,
     public presenceService: PresenceService, private router: Router,
      public timeFormatterService:TimeFormatterService) { }
  ngOnInit(): void {
    this.userProfilePicture = this.user.profilePictureUrl;
  }
  goToUserDetails()
  {
    this.router.navigateByUrl('users/' + this.user.id);
  }
  sendFriendRequest(user: User) {
    this.friendRequestsService.sendFriendRequest(user.id).subscribe(
      r => {
        this.toastr.success('You have sent frined request to ' + user.firstName);
        if (r == true) {
          this.toastr.success("You have a new friend!")
        }
        this.friendRequested.emit();
      }
    );
  }
}
