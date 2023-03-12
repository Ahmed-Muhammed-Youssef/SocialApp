import { BreakpointObserver } from '@angular/cdk/layout';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { LoginResponse } from './_models/AccountModels';
import { AccountService } from './_services/account.service';
import { PresenceService } from './_services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public title = 'Dating App';

  isMobilePhone: boolean = false;

  constructor(public accountService: AccountService, private presenceService: PresenceService,
    private breakpointObserver: BreakpointObserver) {
    breakpointObserver.observe(["(max-width: 700px)"])
      .subscribe(
        result => {
          this.isMobilePhone = false;
          if (result.matches) {
            this.isMobilePhone = true;
          }
        }
      );
  }

  ngOnInit(): void {
    this.setCurrentUser();
  }
  setCurrentUser() {
    let userString = localStorage.getItem('user');
    if (userString != null) {
      let user = JSON.parse(userString) as LoginResponse;
      this.accountService.setCurrentUser(user);
      this.presenceService.createHubConnection(user);
    }
  }
}

