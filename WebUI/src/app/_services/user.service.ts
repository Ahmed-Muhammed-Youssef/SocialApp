import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { Pictures, UpdateUser, User } from '../_models/User';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient, private accountService: AccountService) {
    this.resetUserParams();
  }
  private users: User[] = []; //used for profile caching
  private userParams: UserParams | null = null; //used for filter caching
  private paginationInfo: Pagination =
    {
      currentPage: 0,
      itemsPerPage: 0,
      totalItems: 0,
      totalPages: 0
    };
  private usersChache = new Map<string, PaginatedResult<User[]>>();
  public getUserParams() {
    return this.userParams;
  }
  // public setUserParams(params: UserParams) {
  //   this.userParams = params;
  // }
  public resetUserParams() {
    this.accountService.currentUser$.pipe(take(1)).subscribe(response => {
      if (response) {
        this.userParams = new UserParams(response.userData);
      }
    });
    return this.userParams;
  }

  // helper method
  private getPaginationParams(userParams: UserParams) {
    let httpParams: HttpParams = new HttpParams()
      .set('pageNumber', userParams.pageNumber)
      .set('itemsPerPage', userParams.itemsPerPage)
      .set('sex', userParams.sex)
      .set('orderBy', userParams.orderBy);
    if (userParams.minAge) {
      httpParams = httpParams.set('minAge', userParams.minAge);
    }
    if (userParams.maxAge) {
      httpParams = httpParams.set('maxAge', userParams.maxAge);
    }
    return httpParams;
  }
  public deleteCachedValues() {
    this.usersChache = new Map();
    this.resetUserParams();
  }
  public getAllUsers(userParams: UserParams): Observable<PaginatedResult<User[]>> {
    let paginatedResult: PaginatedResult<User[]> = { result: [], pagination: this.paginationInfo };
    let cache = this.usersChache.get(Object.values(userParams).join('-'));
    if (cache) {
      paginatedResult = cache;
      return of(paginatedResult);
    }
    return this.http.get<User[]>(this.baseUrl + 'users/all', { observe: 'response', params: this.getPaginationParams(userParams) })
      .pipe(
        map(response => {
          if (response?.body) {
            paginatedResult.result = response.body;
          }
          if (response.headers.get('Pagination')) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') as string) as Pagination;
          }
          this.usersChache.set(Object.values(userParams).join('-'), paginatedResult);
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
    return this.http.get<User>(this.baseUrl + 'users/info/' + username);
  }
  public reorderPhotos(photos: Pictures[]) {
    return this.http.put<Pictures[]>(this.baseUrl + 'users/photos/reorder', photos);
  }
  public deletePicture(pictureId: number) {
    return this.http.delete(this.baseUrl + 'users/photo/delete/' + String(pictureId));
  }
  sendFriendRequest(username: string): Observable<boolean> {
    return this.http.post<boolean>(this.baseUrl + 'friendrequests/send/' + username, {}).pipe(map(r => {
      //the correct answer will need a more complex caching system so we will only delete all the cashed data for now
      this.usersChache = new Map();
      return r;
    }));
  }
  getSentFriendRequests(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl + 'friendrequests/sent');
  }
  isFriendRequested(username: string): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'friendrequests/isSent/' + username);
  }
  isFriend(username: string): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + 'friends/isfriend/' + username);
  }
  getFriends(pageNumber: number = 1, itemsPerPage: number = 2): Observable<PaginatedResult<User[]>> {
    let paginatedResult: PaginatedResult<User[]> = { result: [], pagination: this.paginationInfo };
    let httpParams: HttpParams = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('itemsPerPage', itemsPerPage);
    let cache = this.usersChache.get(pageNumber + '-' + itemsPerPage);
    if (cache) {
      paginatedResult = cache;
      return of(paginatedResult);
    }
    return this.http.get<User[]>(this.baseUrl + 'friends', { observe: 'response', params: httpParams })
      .pipe(
        map(response => {
          if (response?.body) {
            paginatedResult.result = response.body;
          }
          if (response.headers.get('Pagination')) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination') as string) as Pagination;
          }
          this.usersChache.set(pageNumber + '-' + itemsPerPage, paginatedResult);
          this.paginationInfo = paginatedResult.pagination;
          return paginatedResult;
        }));

  }
  updateUser(user: User): Observable<UpdateUser> {
    const userTosend: UpdateUser = {
      firstName: user.firstName,
      lastName: user.lastName,
      interest: user.interest,
      bio: user.bio,
      city: user.city,
      country: user.country
    };
    return this.http.put<UpdateUser>(this.baseUrl + 'users/update', userTosend).pipe(map(
      () => {
        const index = this.users.indexOf(user);
        this.users[index] = user;
        return user;
      }
    ));
  }
  getPictures() {
    return this.http.get<Pictures[]>(this.baseUrl + "users/photos/all");
  }
}
