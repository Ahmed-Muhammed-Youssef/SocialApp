import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { take } from 'rxjs';
import { LoginResponse } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';

@Directive({
  selector: '[appHasRole]' // *appHasRole
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole : string[] = [];
  loginResponse: LoginResponse | null = null;
  constructor(private viewContainerRef: ViewContainerRef,
     private templateRef: TemplateRef<any>, private accountService:AccountService) { 
      this.accountService.currentUser$.pipe(take(1)).subscribe(
        l => {
          this.loginResponse = l;
        }
      );
  }
  ngOnInit(): void {
    // clear the view
    if(this.loginResponse === null || this.loginResponse.userData.roles === null){
      this.viewContainerRef.clear();
      return;
    }
    if(this.loginResponse?.userData){
      let roles = this.loginResponse.userData.roles as string[];
      if(roles.some(role => this.appHasRole.includes(role))){
        this.viewContainerRef.createEmbeddedView(this.templateRef);
      }
      else {
        this.viewContainerRef.clear();
        return;
      }
    }
  }

}
