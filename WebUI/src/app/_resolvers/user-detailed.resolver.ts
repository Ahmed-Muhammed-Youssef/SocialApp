import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../_models/User';
import { UserService } from '../_services/user.service';

@Injectable({
  providedIn: 'root'
})
export class UserDetailedResolver  {
  constructor(private userService: UserService){

  }
  resolve(route: ActivatedRouteSnapshot): Observable<User> {
    var val : string | null = route.paramMap.get("id");
    if(val == null){
      console.error("not valid id");
      // I don't know what to do here
      // @TODO: needs to be changed
      return this.userService.getUserById(1);
    }
    else{
      return this.userService.getUserById(+val);
    }
  }
}
