import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {

  constructor(private userService: UserService) { }
  public users: User[] = [];
  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers():void {
    this.userService.getAllUsers().subscribe(
      response => {
        if (response) {
          this.users = response;
        }
      },
      error => {
        console.error(error);
      }
    );
  }

}
