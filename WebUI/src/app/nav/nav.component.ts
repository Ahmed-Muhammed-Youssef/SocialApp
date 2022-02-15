import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginModel } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) {
  }
  public loginCred: LoginModel = { email: '', password: '' };
  ngOnInit(): void {
   
  }
  login(): void {
    this.accountService.login(this.loginCred).subscribe(
      response => {
        this.router.navigateByUrl('/members');
      },
      (error:HttpErrorResponse) => {
        console.log(error);
        this.toastr.error(error.message);
      });
  }
  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/home');
  }
}


