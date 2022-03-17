import { HttpClient, HttpParams, HttpResponse, JsonpClientBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, take } from 'rxjs';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { Photo, UpdateUser, User } from '../_models/User';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient, private accountService: AccountService) { 

    this.resetUserParams();
  }
  private users: User[] = []; //used for profile caching
  private userParams : UserParams| null = null; //used for filter caching
  private paginationInfo : Pagination = 
  { 
    currentPage: 0,
    itemsPerPage: 0,
    totalItems: 0,
    totalPages: 0 
  };
  private userChache = new Map<string, User[]>();
  public getUserParams(){
    return this.userParams;
  }
  public setUserParams(params: UserParams){
    this.userParams = params;
  }
  public resetUserParams(){
    this.accountService.currentUser$.pipe(take(1)).subscribe(response  => {
      if (response) {
        this.userParams = new UserParams(response.userData);
      }
    });
    return this.userParams;
  }

  // helper method
  private getPaginationParams(userParams: UserParams){
    let httpParams: HttpParams = new HttpParams()
    .set('pageNumber',userParams.pageNumber)
    .set('itemsPerPage', userParams.itemsPerPage)
    .set('sex', userParams.sex)
    .set('orderBy', userParams.orderBy);
    if(userParams.minAge){
      httpParams = httpParams.set('minAge', userParams.minAge);
    }
    if(userParams.maxAge){
      httpParams = httpParams.set('maxAge', userParams.maxAge);
    }
    return httpParams;
  }
  public deleteCachedValues(){
    this.userChache = new Map();
    this.resetUserParams();
  }
  public getAllUsers(userParams: UserParams):Observable<PaginatedResult<User[]>> {
    let paginatedResult : PaginatedResult<User[]> =  {result: [], pagination: this.paginationInfo};
    let cache = this.userChache.get(Object.values(userParams).join('-'));
    if(cache){
      paginatedResult.result = cache;
      return of(paginatedResult);
    }

    return this.http.get<User[]>('/api/users/all', { observe: 'response', params: this.getPaginationParams(userParams)})
     .pipe(
      map(response => {
        if(response?.body){
          paginatedResult.result = response.body;
        }
        if(response.headers.get('Pagination')){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') as string) as Pagination;
        }
        this.userChache.set(Object.values(userParams).join('-'), paginatedResult.result);
        this.paginationInfo = paginatedResult.pagination;
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
  like(username: string): Observable<boolean>{
    return this.http.post<boolean>('api/like/' + username, {}).pipe( map( r=> {
      //the correct answer will need a more complex caching system so we will only delete all the cashed data for now
      this.userChache = new Map();
      return r;
    }));
  }
  getLikes() : Observable<User[]>{
    return this.http.get<User[]>('api/like/liked');

  }
  getIsLiked(username:string) : Observable<boolean>{
    return this.http.get<boolean>('/api/like/isliked/' + username);

  }
  getIsMatch(username:string): Observable<boolean>{
    return this.http.get<boolean>('/api/matches/ismatch/' + username);

  }
  getMatches(pageNumber : number = 1, itemsPerPage: number = 2): Observable<PaginatedResult<User[]>>{
    let paginatedResult : PaginatedResult<User[]> =  {result: [], pagination: this.paginationInfo};
    let httpParams: HttpParams = new HttpParams()
    .set('pageNumber', pageNumber)
    .set('itemsPerPage', itemsPerPage);
    let cache = this.userChache.get(pageNumber + '-' + itemsPerPage);
    if(cache){
      paginatedResult.result = cache;
      return of(paginatedResult);
    }
    return this.http.get<User[]>('api/matches', {observe:'response', params: httpParams})
    .pipe(
      map(response => {
        if(response?.body){
          paginatedResult.result = response.body;
        }
        if(response.headers.get('Pagination')){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') as string) as Pagination;
        }
        this.userChache.set(pageNumber + '-' + itemsPerPage, paginatedResult.result);
        this.paginationInfo = paginatedResult.pagination;
        return paginatedResult;
    }));

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
