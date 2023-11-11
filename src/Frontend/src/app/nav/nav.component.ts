import { BreakpointObserver } from '@angular/cdk/layout';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginResponse } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  isMobilePhone : boolean = false;
  public userProfilePicture: string | undefined;
  constructor(public accountService: AccountService,
     private router: Router, breakpointObserver: BreakpointObserver) {
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
          this.userProfilePicture = account.userData.profilePictureUrl;
        }
      });
  }
  logout(): void {
    this.accountService.logout();
    this.router.navigateByUrl('/login');
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


