import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../_models/User';
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
      breakpointObserver.observe(["(max-width: 770px)"])
        .subscribe(
          result => {
            this.isMobilePhone = false;
            if (result.matches) {
              this.isMobilePhone = true;
            }
          }
        );
    // this.galleryOptions = [
    //   {
    //     width: '500px',
    //     height: '500px',
    //     imagePercent: 100,
    //     thumbnailsColumns: 4,
    //     imageAnimation: NgxGalleryAnimation.Slide,
    //     preview: false
    //   }
    // ];
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
    // for (const photo of this.user.photos) {
    //   this.galleryImages.push({
    //     small: photo?.url,
    //     medium: photo?.url,
    //     big: photo?.url
    //   });
    // }
  }

}
