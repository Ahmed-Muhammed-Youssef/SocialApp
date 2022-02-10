import { HttpClient } from '@angular/common/http';
import { OnInit } from '@angular/core';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public title = 'Dating App';
  public users?: User[];

  constructor(private http: HttpClient) {
  }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers():void {
    this.http.get<User[]>('/api/users').subscribe(
      response => { this.users = response; },
      error => console.log(error)
    );
  }

}
interface User {
  firstName: string;
  lastName: string;
  sex: string;
  interest: string;
}
