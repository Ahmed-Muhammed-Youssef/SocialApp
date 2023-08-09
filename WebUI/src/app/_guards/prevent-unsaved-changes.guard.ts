import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { UserEditComponent } from '../users/user-edit/user-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard  {
  canDeactivate(component: UserEditComponent): boolean {
    if (component.checkIfUserChanged()) {
      return confirm('Are you sure you want to discard the unsaved changes?');
    }
    return true;
  }
  
}
