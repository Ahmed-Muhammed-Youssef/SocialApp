import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';
import { UserService } from '../_services/user.service';
import { StaticDataService } from '../_services/staticData.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  public registerForm: UntypedFormGroup = this.formBuilder.group({
    username: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
    country: ['', Validators.required],
    city:['', Validators.required],
    email: ['', Validators.required],
    sex: ['', Validators.required],
    interest: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required, this.passwordConfirming]]
  });
  public maxDate:Date = new Date();
  constructor(private accountService: AccountService, private toastr: ToastrService,
     private formBuilder: UntypedFormBuilder, private router: Router, private userService: UserService,
     public staticDataService : StaticDataService) {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() - 18);
  }
  public initializeForm(){
    this.registerForm.controls.password.valueChanges.subscribe((value) => 
    {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }
  // custom validator of type ValidatorFn
  public passwordConfirming(control: AbstractControl): { passwordMismatch: boolean } | null {
    if(control){
      if(control.value != control.parent?.get('password')?.value){
        return {passwordMismatch: true};
      }
    }
    return null;
  }
  public ngOnInit(): void {
  }
  public register(): void {
    this.accountService.register(this.registerForm.value).subscribe(
      response => {
        if(response){
          this.router.navigateByUrl('users');
          this.userService.deleteCachedValues();
        }
      },
      (error: HttpErrorResponse) => {
      
       });
  }
}
