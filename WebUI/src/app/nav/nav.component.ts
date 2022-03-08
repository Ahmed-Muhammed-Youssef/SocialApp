import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginModel } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';
import { UserService } from '../_services/user.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private userService: UserService, public accountService: AccountService, private router: Router, private toastr: ToastrService) {
  }
  ngOnInit(): void {  }
  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/home');
  }
}


