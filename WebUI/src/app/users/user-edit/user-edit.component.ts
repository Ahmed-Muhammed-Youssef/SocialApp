import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { LoginResponse } from '../../_models/AccountModels';
import { User } from '../../_models/User';
import { AccountService } from '../../_services/account.service';
import { UserService } from '../../_services/user.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  public user: User | undefined;
  public account: LoginResponse | undefined;
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(private toastr: ToastrService, private userService: UserService, private accountService: AccountService) {
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
      });
    }
  }
  updateUser() {
    if (this.user) {
      this.userService.updateUser(this.user).subscribe(r => {
        if (r && this.user) {
          this.toastr.success('Profile updated successfully');
          this.editForm?.reset(this.user);
        }
      });
    }
  }
}
