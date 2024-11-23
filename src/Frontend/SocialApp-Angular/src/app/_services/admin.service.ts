import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { RoleUser } from '../_models/roles';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  baseUrl = "https://localhost:5001";
  cachedRoles: string[] = [];
  constructor(private http: HttpClient) { }
  getUsersWithRoles(){
    return this.http.get<RoleUser[]>(this.baseUrl +"roles/users-roles/all");
  }
  getAllRoles(){
    if(this.cachedRoles.length > 0){
      return of(this.cachedRoles)
    }
    return this.http.get<string[]>(this.baseUrl + "roles/all").pipe(
      map( r => 
      {
        this.cachedRoles = r;
        return r;
      }));
  }
  addRoleToUser(username: string, role:string){
    let obj = {'username': username, 'role': role};
    return this.http.post<string>(this.baseUrl + 'roles/add', obj);
  }
  deleteRoleFromUser(username: string, role:string){
    let obj = {'username': username, 'role': role};
    return this.http.delete<string>(this.baseUrl + 'roles/removefrom', {body: obj});
  }
}
