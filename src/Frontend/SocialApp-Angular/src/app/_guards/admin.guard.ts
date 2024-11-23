import { map, Observable } from 'rxjs';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export class AdminGuard  {
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