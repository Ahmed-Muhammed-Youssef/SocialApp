import { Component, OnInit } from '@angular/core';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { User } from '../_models/User';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  pagination : Pagination = {
    itemsPerPage: 4,
    currentPage: 1,
    totalItems:0,
    totalPages:0
  } ;


  matches: User[] = [];
  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadMatches();
  }
  loadMatches(){
    this.userService.getMatches(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe({
      next: response => {
        if (response) {
          if(response){
            this.matches = response.result;
            this.pagination = response.pagination;
          }
        }
      }});
  }
  pageChanged($event: any){
    if($event){
      this.pagination.currentPage = $event.pageIndex + 1;
      this.loadMatches();
    }
  }

}
