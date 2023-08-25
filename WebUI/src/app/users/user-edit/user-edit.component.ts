import { BreakpointObserver } from '@angular/cdk/layout';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { LoginResponse } from '../../_models/AccountModels';
import { User } from '../../_models/User';
import { AccountService } from '../../_services/account.service';
import { UserService } from '../../_services/user.service';
import { StaticDataService } from 'src/app/_services/staticData.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  public user: User | undefined;
  public isMobilePhone : boolean = false;
  public oldUser: User | undefined;
  public account: LoginResponse | undefined;
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.checkIfUserChanged()) {
      $event.returnValue = true;
    }
  }
  constructor(private toastr: ToastrService, private userService: UserService, 
    private accountService: AccountService, private breakpointObserver: BreakpointObserver,
    public staticDataService : StaticDataService) {
    breakpointObserver.observe(["(max-width: 750px)"])
      .subscribe(
        result => {
          this.isMobilePhone = false;
          if (result.matches) {
            this.isMobilePhone = true;
          }
        }
      );
    this.accountService.currentUser$.pipe(take(1)).subscribe(account => {
      if (account) {
        this.account = account;
      }
    });
  }

  ngOnInit(): void {
    this.loadUser();
  }
  loadUser() {
    if (this.account?.userData) {
      this.userService.getUserByUsername(this.account?.userData.username).subscribe(user => {
        this.user = user;
        this.oldUser = Object.assign({}, this.user);
      });
    }
  }
  checkIfUserChanged(){
    let prop : keyof User;
    if(this.oldUser && this.user){
      for(prop in this.oldUser){
        if(this.oldUser[prop] !== this.user[prop]){
          return true;
        }
      }
    }
    return false;
  }
  public getLoacaleDateTime(d: Date) : Date{
    var localDate  = new Date(d.toString() + 'Z');
    return localDate;
  }
  updateUser() {
    if (this.user) {
      this.userService.updateUser(this.user).subscribe(r => {
        if (r && this.user) {
          this.toastr.success('Profile updated successfully');
          this.oldUser = Object.assign({}, this.user);
          this.editForm?.reset(this.user);
        }
      });
    }
  }
  
}
