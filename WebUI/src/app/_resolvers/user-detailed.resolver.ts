import { Injectable } from '@angular/core';
import {
  Resolve,
  ActivatedRouteSnapshot
} from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { UserService } from '../_services/user.service';

@Injectable({
  providedIn: 'root'
})
export class UserDetailedResolver implements Resolve<User> {
  constructor(private userService: UserService){

  }
  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    return this.userService.getUserByUsername(route.paramMap.get("username") as string);
  }
}
