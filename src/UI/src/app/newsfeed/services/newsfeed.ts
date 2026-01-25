import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { PostDTO } from '../models/post-dto';
import { CreatePostRequest } from '../models/create-post-request';
import { PagedList } from '../../shared/models/paged-list';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class NewsfeedService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/posts`;

  // GET /api/posts?userId=123
  getUserPosts(userId: number): Observable<PostDTO[]> {
    return this.http.get<PostDTO[]>(`${this.baseUrl}?userId=${userId}`);
  }

  getPosts(pageNumber: number, itemsPerPage: number): Observable<PagedList<PostDTO>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('itemsPerPage', itemsPerPage);

    return this.http.get<PagedList<PostDTO>>(this.baseUrl, { params });
  }

  // GET /api/posts/{postId}
  getPostById(postId: number): Observable<PostDTO> {
    return this.http.get<PostDTO>(`${this.baseUrl}/${postId}`);
  }

  // POST /api/posts
  // Returns the ID extracted from the Location header (RESTful best practice)
  createPost(request: CreatePostRequest): Observable<number> {
    return this.http.post(this.baseUrl, request, { observe: 'response' }).pipe(
      map(response => {
        const locationHeader = response.headers.get('Location');
        if (!locationHeader) {
          throw new Error('Location header missing from POST response');
        }
        // Extract ID from Location header (e.g., "/api/posts/123" -> 123)
        const postId = parseInt(locationHeader.split('/').pop()!, 10);
        return postId;
      })
    );
  }
}
