import { Component, OnInit } from '@angular/core';
import { LoginResponse } from 'src/app/_models/AccountModels';
import { Pagination } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  pagination : Pagination | null = null ;
  userParams: UserParams | null = null;
  genderList = [{value: 'm', display: 'Males'}, {value: 'f', display: 'Females'}, {value: 'b', display: 'Both'}];
  orderByOptions = [{value: 'lastActive', display: 'Last Active'}, {value: 'creationTime', display: 'Newest Users'}, {value: 'age', display: 'Age'} ];
  // public users$: Observable<User[]>;

  constructor(private userService: UserService) {
   this.userParams = userService.getUserParams();
  }
  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers(){
    this.userService.setUserParams(this.userParams as UserParams);
    this.userService.getAllUsers(this.userParams as UserParams).subscribe(
      response => {
        if(response){
          this.users = response.result;
          this.pagination = response.pagination;
        }
      }
    );
  }
  resetFilters(){
    this.userParams = this.userService.resetUserParams();
    this.loadUsers();
  }
  pageChanged($event:any){
    if(event){
      if($event && this.userParams){
        this.userParams.pageNumber = $event.page;
        this.userService.setUserParams(this.userParams);
        this.loadUsers();
      }
    }
  }
  
  

}
