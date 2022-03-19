import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/_services/user.service';
import { User } from '../../_models/User';

@Component({
  selector: 'app-user-card',
  templateUrl: './user-card.component.html',
  styleUrls: ['./user-card.component.css']
})
export class UserCardComponent implements OnInit {
  @Input () isLiked = false;
  @Output () liked = new EventEmitter();
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
 
  constructor(private userService: UserService, private toastr: ToastrService) { }
  ngOnInit(): void {
  }
  addLike(user: User){
    this.userService.like(user.username).subscribe(
      r => {
          this.toastr.success('You have liked ' + user.firstName);
          if(r == true){
            this.toastr.success("You have a new match!")
          }
          this.liked.emit();
      }
    );
  }
}
