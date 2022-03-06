import { HttpClient, HttpParams, HttpResponse, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { Photo, UpdateUser, User } from '../_models/User';
import { UserParams } from '../_models/userParams';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient) { }
  public users: User[] = [];
  public paginationInfo : Pagination = 
  { 
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 10,
    totalPages: 1 
  }
  

  // helper method
  private getPaginationParams(userParams: UserParams){
    let httpParams: HttpParams = new HttpParams()
    .set('pageNumber',userParams.pageNumber)
    .set('itemsPerPage', userParams.itemsPerPage)
    .set('sex', userParams.sex);
    if(userParams.minAge){
      httpParams = httpParams.set('minAge', userParams.minAge);
    }
    if(userParams.maxAge){
      httpParams = httpParams.set('maxAge', userParams.maxAge);
    }
    return httpParams;
  }
  public getAllUsers(userParams: UserParams):Observable<PaginatedResult<User[]>> {
    let paginatedResult : PaginatedResult<User[]> =  {result: [], pagination: this.paginationInfo};
    return this.http.get<User[]>('/api/users/all', { observe: 'response', params: this.getPaginationParams(userParams)})
     .pipe(
      map(response => {
        if(response?.body){
          paginatedResult.result = response.body;
        }
        if(response.headers.get('Pagination')){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') as string) as Pagination;
        }
        return paginatedResult;
      })
    );
  }
  public getUserByUsername(username: string): Observable<User> {
    const user = this.users.find(u => u.username === username);
    if (user) {
      return of(user);
    }
    return this.http.get<User>('/api/users/info/username/' + username);
  }
  public reorderPhotos(photos: Photo[]){
    return this.http.put<Photo[]>('/api/users/photos/reorder', photos);
  }
  public deletePhoto(photoId:number){
    return this.http.delete('/api/users/photo/delete/' + String(photoId));
  }
  updateUser(user: User): Observable<UpdateUser> {
    const userTosend: UpdateUser  = {
      firstName: user.firstName,
      lastName: user.lastName,
      interest: user.interest,
      bio: user.bio,
      city: user.city,
      country: user.country
    };
    return this.http.put<UpdateUser>('/api/users/update', userTosend).pipe(map(
      () => {
        const index = this.users.indexOf(user);
        this.users[index] = user;
        return user;
      }
    ));
  }
}
