import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  public users$: Observable<User[]>;
  constructor(private userService: UserService) {
    this.users$ = this.userService.getAllUsers();
  }
  ngOnInit(): void {
    
  }
  
  

}
