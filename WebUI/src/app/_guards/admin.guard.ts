import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable, of } from 'rxjs';
import { LoginResponse } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastrService: ToastrService){

  }
  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(login  => {

        if(login?.userData.roles.includes('admin') || login?.userData.roles.includes('moderator')){
          return true;
        }
        this.toastrService.error("Unauthorized Access");
        return false;
      })
    );
  }
  
}
