import { Component, OnInit } from '@angular/core';
import { RegisterModel } from '../_models/AccountModels';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  constructor(private accountService: AccountService) {
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
      error => console.log(error)
    );
  }
}
