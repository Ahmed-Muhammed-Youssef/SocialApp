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

  //public users?: User[];

  constructor(private accountService: AccountService, private presenceService: PresenceService) {
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

