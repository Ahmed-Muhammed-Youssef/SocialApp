import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.css']
})
export class TestErrorComponent implements OnInit {
  validationErrors: string[] = [];
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }
  get404Error(): void {
    this.http.get('/api/buggy/not-found').subscribe(
      response => {
        console.log(response);

      },
      error => {
        console.log(error);

      }
    );
  }
  get400Error(): void {
    this.http.get('/api/buggy/bad-request').subscribe(
      response => {
        console.log(response);
      },
      error => {
        console.log(error);

      }
    );
  }
  get401Error(): void {
    this.http.get('/api/buggy/auth').subscribe(
      response => {
        console.log(response);

      },
      error => {
        console.log(error);

      }
    );
  }
  get500Error(): void {
    this.http.get('/api/buggy/server-error').subscribe(
      response => {
        console.log(response);

      },
      error => {
        console.log(error);

      }
    );
  }
  get400ValidationError(): void {
    this.http.post('/api/account/register', {}).subscribe(
      response => {
        console.log(response);

      },
      error => {
        console.log(error);
        this.validationErrors = error;
      }
    );
  }

}
