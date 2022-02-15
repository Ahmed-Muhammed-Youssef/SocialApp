import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { map, Observable } from 'rxjs';
import { LoginResponse } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private accountService: AccountService, private toastr: ToastrService) {

  }
  canActivate(): Observable<boolean>{
    return this.accountService.currentUser$.pipe(
      map<LoginResponse | null, boolean>((response: LoginResponse | null) => {
        if (response) {
          return true;
        }
        else {
          this.toastr.error("You need to login!");
          return false;
        }
      })
    );
  }
  
}
