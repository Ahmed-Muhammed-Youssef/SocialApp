import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { RegisterModel } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private accountService: AccountService, private toastr: ToastrService) {
    this.registerModel = { email: '', password: '', sex: '', interest: '', firstName: '', lastName: '' };
  }
  public registerModel: RegisterModel;
  ngOnInit(): void {
  }
  register(): void {
    this.accountService.register(this.registerModel).subscribe(
      response => {
        console.log(response);
      },
      (error: HttpErrorResponse) => {
        console.log(error.message);
        this.toastr.error(error.error);
      }
    );
  }
}
