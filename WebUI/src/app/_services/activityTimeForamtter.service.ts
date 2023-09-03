import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TimeFormatterService {
  constructor() { }
  public getLoacaleDateTime(d: Date): Date {
    var localDate = new Date(d.toString() + 'Z');
    return localDate;
  }
  public getDateTimeAgo(date: Date) {
    var now = new Date();
    date = this.getLoacaleDateTime(date);
    var yearDiff = now.getFullYear() - date.getFullYear();
    var monthDiff = now.getMonth()- date.getMonth();
    var dayDiff = now.getDate()- date.getDate();
    var hourDiff = now.getHours() - date.getHours();
    var minuteDiff = now.getMinutes() - date.getMinutes();
    if(yearDiff > 0)
    {
      return yearDiff + ' year'+ (yearDiff> 1? 's':'') + ' ago';
    }
    else 
    if (monthDiff > 0){
      return monthDiff + ' month'+ (monthDiff> 1? 's':'') + ' ago';
    }
    else if (dayDiff > 0){
      return dayDiff + ' day'+ (dayDiff> 1? 's':'') + ' ago';
    }
    else if (hourDiff > 0){
      return hourDiff + ' hour'+ (hourDiff> 1? 's':'') + ' ago';
    }
    else if (minuteDiff > 0){
      return minuteDiff + ' minute'+ (minuteDiff> 1? 's':'') + ' ago';
    }
    else {
      return 'less than a minute';
    }
  }
}
