import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';
import { NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { NgxGalleryImage } from '@kolkov/ngx-gallery';
import { NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
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
    photos: []
  };
  isLiked : boolean = false;
  username: string = '';
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  constructor(private userService: UserService, private route: ActivatedRoute, private toastr: ToastrService) {
    this.route.paramMap.subscribe(param => {
      this.username = String(param.get('username'));
    });
    this.loadUser();
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];
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
    
  }
  loadUser() {
    this.userService.getUserByUsername(this.username).subscribe(u => {
      this.user = u;
      this.setImages();
    });
    this.userService.getIsLiked(this.username).subscribe(r => {
      this.isLiked = r;
    });
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


  public setImages() {
    for (const photo of this.user.photos) {
      this.galleryImages.push({
        small: photo?.url,
        medium: photo?.url,
        big: photo?.url
      });
    }
  }

}
