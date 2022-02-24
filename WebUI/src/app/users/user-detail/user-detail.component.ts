import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';
import { NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { NgxGalleryImage } from '@kolkov/ngx-gallery';
import { NgxGalleryAnimation } from '@kolkov/ngx-gallery';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
  user: User = {
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
  username: string = '';
  galleryOptions: NgxGalleryOptions[] = [];
  galleryImages: NgxGalleryImage[] = [];
  constructor(private userService: UserService, private route: ActivatedRoute) {
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
  
  ngOnInit(): void {
    
  }
  loadUser() {
    this.userService.getUserByUsername(this.username).subscribe(u => {
      this.user = u;
      this.setImages();
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
