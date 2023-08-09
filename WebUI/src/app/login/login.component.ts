import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginModel, LoginResponse } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginValid = true;
  constructor(public accountService: AccountService, private router: Router, private userService: UserService) { }
  public loginCred: LoginModel = { email: '', password: '' };
  ngOnInit(): void {
  }
  public login() {
    this.accountService.login(this.loginCred).subscribe({
      next: response => {
        if (response) {
          if(response.token){
            this.router.navigateByUrl('/users');
            this.userService.deleteCachedValues();
          }
        }
      },
      error: (e) => this.loginValid = false
    });
  }
}
