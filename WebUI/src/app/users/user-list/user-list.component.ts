import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { LoginResponse } from 'src/app/_models/AccountModels';
import { Pagination } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  account: LoginResponse | null = null;
  pagination : Pagination | null = null ;
  userParams: UserParams | null = null;
  genderList = [{value: 'm', display: 'Males'}, {value: 'f', display: 'Females'}, {value: 'b', display: 'Both'}];
  orderByOptions = [{value: 'lastActive', display: 'Last Active'}, {value: 'creationTime', display: 'Newest Users'}, {value: 'age', display: 'Age'} ];
  // public users$: Observable<User[]>;

  constructor(private userService: UserService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(response  => {
      if (response) {
        this.account = response;
        this.userParams = new UserParams(response.userData);
      }
    });
  }
  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers(){
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
    this.userParams = new UserParams(this.account?.userData as User);
    this.loadUsers();
  }
  pageChanged($event:any){
    if(event){
      if($event && this.userParams){
        this.userParams.pageNumber = $event.page;
        this.loadUsers();
      }
    }
  }
  
  

}
