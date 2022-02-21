import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginModel } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { }
  public loginCred: LoginModel = { email: '', password: '' };
  ngOnInit(): void {
  }
  public login() {
    this.accountService.login(this.loginCred).subscribe(
      response => {
        if (response) {
          this.router.navigateByUrl('/users');
        }
      },
      (error: HttpErrorResponse) => {
        console.log(error);
      });
  }

}
