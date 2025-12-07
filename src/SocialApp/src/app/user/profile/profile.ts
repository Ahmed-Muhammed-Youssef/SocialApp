import { Component, inject, signal } from '@angular/core';
import { UsersService } from '../services/users';
import { ActivatedRoute } from '@angular/router';
import { UserDTO } from '../../auth/models/user-dto';
import { MatCardModule } from '@angular/material/card';
import { DatePipe } from '@angular/common';
import { PostItem } from '../../newsfeed/post-item/post-item';
import { PostDTO } from '../../newsfeed/models/post-dto';

@Component({
  selector: 'app-profile',
  imports: [MatCardModule, DatePipe, PostItem],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile {
  private userService = inject(UsersService);
  private route = inject(ActivatedRoute);

  user = signal<UserDTO | null>(null);
  posts = signal<PostDTO[]>([]);
  defaultImage = 'imgs/default-user.png';

  constructor()
  {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.userService.getUserById(id).subscribe({
      next: (res) => this.user.set(res),
      error: (err) => console.error(err)
    });

    // Get user posts
    this.userService.getUserPosts(id).subscribe({
      next: res => this.posts.set(res),
      error: err => console.error(err)
    });
  }

}
