import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  constructor(/*private accountService: AccountService, private router: Router*/) { }
  ngOnInit(): void {
    /*if (this.accountService.currentUser$) {
      this.router.navigateByUrl('/users');
    }*/
  }
  registerToggle() {
  }
}
