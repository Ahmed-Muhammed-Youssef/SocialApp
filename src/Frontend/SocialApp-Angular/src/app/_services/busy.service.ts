import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class BusyService {
  public busyRequestCount: number = 0;
  constructor(private spinnerService: NgxSpinnerService) { }
  public busy() {
    this.busyRequestCount++;
    this.spinnerService.show();
  }
  public idle() {
    this.busyRequestCount--;
    if (this.busyRequestCount < 1) {
      this.busyRequestCount = 0; // for safety just in case
      this.spinnerService.hide();
    }
  }
}
