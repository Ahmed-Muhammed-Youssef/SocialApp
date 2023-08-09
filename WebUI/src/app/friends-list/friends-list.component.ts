import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_models/pagination';
import { User } from '../_models/User';
import { UserService } from '../_services/user.service';
import { BreakpointObserver } from '@angular/cdk/layout';

@Component({
  selector: 'app-lists',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.css']
})
export class FriendsListComponent implements OnInit {
  pagination: Pagination = {
    itemsPerPage: 4,
    currentPage: 1,
    totalItems: 0,
    totalPages: 0
  };

  isMobilePhone: boolean = false;

  matches: User[] = [];
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
  }

  ngOnInit(): void {
    this.loadMatches();
  }
  loadMatches() {
    this.userService.getMatches(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe({
      next: response => {
        if (response) {
          if (response) {
            this.matches = response.result;
            this.pagination = response.pagination;
          }
        }
      }
    });
  }
  pageChanged($event: any) {
    if ($event) {
      this.pagination.currentPage = $event.pageIndex + 1;
      this.loadMatches();
    }
  }

}
