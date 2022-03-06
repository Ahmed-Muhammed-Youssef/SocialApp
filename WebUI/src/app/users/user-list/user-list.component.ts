import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Pagination } from 'src/app/_models/pagination';
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
  currentPage = 1;
  itemsPerPage = 4;

  // public users$: Observable<User[]>;

  constructor(private userService: UserService) {
   
  }
  ngOnInit(): void {
    this.loadUsers();
  }
  loadUsers(){
    
    this.userService.getAllUsers(this.currentPage, this.itemsPerPage).subscribe(
      response => {
        if(response){
          this.users = response.result;
          this.pagination = response.pagination;
        }
      }
    );
  }
  pageChanged($event:any){
    if(event){
      if($event){
        this.currentPage = $event.page;
        this.loadUsers();
      }
    }
  }
  
  

}
