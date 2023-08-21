import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Pictures, User } from '../../_models/User';
import { UserService } from '../../_services/user.service';

import { ToastrService } from 'ngx-toastr';
import { PresenceService } from 'src/app/_services/presence.service';
import { BreakpointObserver } from '@angular/cdk/layout';
import { TimeFormatterService } from 'src/app/_services/activityTimeForamtter.service';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
  isMobilePhone : boolean = false;
  profilePicture : Pictures | undefined;
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
  isLiked : boolean = true;
  isMatch: boolean = false;
  
  constructor(private userService: UserService, private route: ActivatedRoute,
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
  addLike(){
    this.userService.like(this.user.username).subscribe(
      r => {
          this.isLiked = true;
          this.toastr.success('You have liked ' + this.user.firstName);
          if(r == true){
            this.toastr.success("You have a new match!")
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
        this.userService.getIsLiked(this.user.username).subscribe(r => {
          this.isLiked = r;
        });
      this.userService.getIsMatch(this.user.username).subscribe(r => this.isMatch = r);
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
