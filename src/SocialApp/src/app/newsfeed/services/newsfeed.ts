import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { PostDTO } from '../models/post-dto';
import { CreatePostRequest } from '../models/create-post-request';

@Injectable({
  providedIn: 'root',
})
export class NewsfeedService {
  private http = inject(HttpClient);
  private readonly baseUrl = 'https://localhost:5001/api/posts';

  // GET /api/posts?userId=123
  getUserPosts(userId: number): Observable<PostDTO[]> {
    return this.http.get<PostDTO[]>(`${this.baseUrl}?userId=${userId}`);
  }

  // GET /api/posts/{postId}
  getPostById(postId: number): Observable<PostDTO> {
    return this.http.get<PostDTO>(`${this.baseUrl}/${postId}`);
  }

  // POST /api/posts
  createPost(request: CreatePostRequest): Observable<void> {
    return this.http.post<void>(this.baseUrl, request);
  }
}
