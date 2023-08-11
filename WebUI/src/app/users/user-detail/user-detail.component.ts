import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Photo, User } from '../../_models/User';
import { UserService } from '../../_services/user.service';

import { ToastrService } from 'ngx-toastr';
import { PresenceService } from 'src/app/_services/presence.service';
import { BreakpointObserver } from '@angular/cdk/layout';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
  isMobilePhone : boolean = false;
  profilePicture : Photo | undefined;
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
    photos: [],
    roles: []
  };
  isLiked : boolean = true;
  isMatch: boolean = false;
  
  constructor(private userService: UserService, private route: ActivatedRoute,
     private toastr: ToastrService, public presenceService: PresenceService, private breakpointObserver: BreakpointObserver) {
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
  public getLoacaleDateTime(d: Date) : Date{
    var localDate  = new Date(d.toString() + 'Z');
    return localDate;
  }
  ngOnInit(): void {
    this.route.data.subscribe(
      data => {
        this.user = data.user;
        this.profilePicture = this.user.photos[0];
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
