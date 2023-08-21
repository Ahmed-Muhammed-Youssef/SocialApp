import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PresenceService } from 'src/app/_services/presence.service';
import { UserService } from 'src/app/_services/user.service';
import { Pictures, User } from '../../_models/User';
import { Router } from '@angular/router';
import { TimeFormatterService } from 'src/app/_services/activityTimeForamtter.service';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() isFriendRequested = false;
  @Output() liked = new EventEmitter();
  @Input() user: User = {
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
  constructor(private userService: UserService, private toastr: ToastrService,
     public presenceService: PresenceService, private router: Router,
      public timeFormatterService:TimeFormatterService) { }
  ngOnInit(): void {
  }
  goToUserDetails()
  {
    this.router.navigateByUrl('users/username/' + this.user.username);
  }
  sendFriendRequest(user: User) {
    this.userService.sendFriendRequest(user.username).subscribe(
      r => {
        this.toastr.success('You have sent frined request to ' + user.firstName);
        if (r == true) {
          this.toastr.success("You have a new friend!")
        }
        this.liked.emit();
      }
    );
  }
}
