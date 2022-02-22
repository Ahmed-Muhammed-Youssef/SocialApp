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
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(private toastr: ToastrService, private userService: UserService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      if (user) {
        this.user = user.userData;
      }
    });
  }

  ngOnInit(): void {
  }
  loadUser() {
  }
  updateUser() {
    if (this.user) {
      this.userService.updateUser(this.user).subscribe(r => {
        if (r && this.user) {
          this.user.firstName = r.firstName;
          this.user.lastName = r.lastName;
          this.user.bio = r.bio;
          this.user.interest = r.interest;
          this.user.city = r.city;
          this.user.country = r.country;
          this.toastr.success('Profile updated successfully');
          this.editForm?.reset(this.user);
          let dataStored: LoginResponse = JSON.parse(String(localStorage.getItem('user')));
          dataStored.userData = this.user;
          localStorage.removeItem('user');
          localStorage.setItem('user', JSON.stringify(dataStored));
        }
      });
    }
  }
}
