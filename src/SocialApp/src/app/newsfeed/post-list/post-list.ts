import { Component, inject, signal } from '@angular/core';
import { NewsfeedService } from '../services/newsfeed';
import { PostDTO } from '../models/post-dto';
import { AuthService } from '../../auth/services/auth';
import { MatIconModule } from '@angular/material/icon';
import { PostItem } from '../post-item/post-item';

@Component({
  selector: 'app-post-list',
  imports: [MatIconModule, PostItem],
  templateUrl: './post-list.html',
  styleUrl: './post-list.css',
})
export class PostList {
  private newsfeedService = inject(NewsfeedService);
  private authService = inject(AuthService);

  posts = signal<PostDTO[]>([]);

  constructor() {
    const currentUser = this.authService.getUserData();

    if (currentUser) {
      this.newsfeedService.getUserPosts(currentUser.id).subscribe({
        next: posts => {this.posts.set(posts); console.log(posts)},
        error: (err) => {this.posts.set([]); console.error(err)}
      });
    }
  }
}
