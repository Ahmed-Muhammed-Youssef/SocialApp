import { Component, OnInit } from '@angular/core';
import { Pagination } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { User } from '../../_models/User';
import { UserService } from '../../_services/user.service';
import { BreakpointObserver } from '@angular/cdk/layout';

@Component({
  selector: 'app-member-list',
  templateUrl: './explore.component.html',
  styleUrls: ['./explore.component.css']
})
export class ExploreComponent implements OnInit {
  users: User[] = [];
  isMobilePhone: boolean = false;
  pagination: Pagination | null = null;
  userParams: UserParams | null = null;
  genderList = [{ value: 'm', display: 'Males' }, { value: 'f', display: 'Females' }, { value: 'b', display: 'Both' }];
  orderByOptions = [{ value: 'lastActive', display: 'Last Active' }, { value: 'creationTime', display: 'Newest' }, { value: 'age', display: 'Age' }];
  constructor(private userService: UserService, private breakpointObserver: BreakpointObserver) {
    breakpointObserver.observe(["(max-width: 750px)"])
      .subscribe(
        result => {
          this.isMobilePhone = false;
          if (result.matches) {
            this.isMobilePhone = true;
          }
        }
      );
    this.userParams = userService.getUserParams();
  }
  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers() {
    // this.userService.setUserParams(this.userParams as UserParams);
    this.userService.getAllUsers(this.userParams as UserParams).subscribe(
      response => {
        if (response) {
          this.users = response.result;
          this.pagination = response.pagination;
        }
      }
    );
  }
  resetFilters() {
    this.userParams = this.userService.resetUserParams();
    this.loadUsers();
  }
  pageChanged($event: any) {
    if ($event) {
      if ($event && this.userParams) {
        this.userParams.pageNumber = $event.pageIndex + 1;
        // this.userService.setUserParams(this.userParams);
        this.loadUsers();
      }
    }
  }
}
