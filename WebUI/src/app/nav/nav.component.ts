import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginModel, LoginResponse } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(public accountService: AccountService) {
  }
  public loginCred: LoginModel = { email: '', password: '' };
  ngOnInit(): void {
   
  }
  login(): void {
    this.accountService.login(this.loginCred).subscribe(
      response => { console.log(response); },
      error => console.log(error));
  }
  logout(): void {
    this.accountService.logout();
  }
}


