import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PresenceService } from 'src/app/_services/presence.service';
import { UserService } from 'src/app/_services/user.service';
import { Photo, User } from '../../_models/User';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input() isLiked = false;
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
    photos: [],
    roles: []
  };
  constructor(private userService: UserService, private toastr: ToastrService,
     public presenceService: PresenceService, private router: Router) { }
  ngOnInit(): void {
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
  goToUserDetails()
  {
    this.router.navigateByUrl('users/username/' + this.user.username);
  }
  addLike(user: User) {
    this.userService.like(user.username).subscribe(
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
