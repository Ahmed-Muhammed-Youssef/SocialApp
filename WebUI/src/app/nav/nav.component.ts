import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginResponse } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';
import { UserService } from '../_services/user.service';
import { take } from 'rxjs';
import { Photo } from '../_models/User';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  isMobilePhone : boolean = false;
  public userProfilePicture: Photo | undefined;
  constructor(private userService: UserService, public accountService: AccountService,
     private router: Router, private toastr: ToastrService,
     private breakpointObserver: BreakpointObserver) {
      breakpointObserver.observe(["(max-width: 750px)"])
      .subscribe(
        result => 
        {
          this.isMobilePhone = false;
          if(result.matches)
          {
            this.isMobilePhone = true;
          }
        }
      );
      this.accountService.currentUser$.pipe(take(1)).subscribe(account => {
        if (account) {
          this.userProfilePicture = account.userData.photos[0];
        }
      });
  }
  ngOnInit(): void {  }
  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/home');
  }
  route(){
    let userString = localStorage.getItem('user');
    if(userString){
      let user = JSON.parse(userString) as LoginResponse;
      if(user.token){
        this.router.navigateByUrl('/users');
        return;
      }
    }
    this.router.navigateByUrl('/home');
  }
}


