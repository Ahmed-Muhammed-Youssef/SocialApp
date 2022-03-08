import { Component, OnInit } from '@angular/core';
import { User } from '../_models/User';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  matches: User[] = [];
  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.loadMatches();
  }
  loadMatches(){
    this.userService.getMatches().subscribe(
      r => {
        this.matches = r;
      }
    );
  }

}
