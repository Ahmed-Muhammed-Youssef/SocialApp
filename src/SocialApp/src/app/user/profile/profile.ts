import { Component, inject, signal } from '@angular/core';
import { UsersService } from '../services/users';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { UserDTO } from '../../auth/models/user-dto';
import { MatCardModule } from '@angular/material/card';
import { DatePipe } from '@angular/common';
import { PostItem } from '../../newsfeed/post-item/post-item';
import { PostDTO } from '../../newsfeed/models/post-dto';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../auth/services/auth';

@Component({
  selector: 'app-profile',
  imports: [MatCardModule, DatePipe, PostItem, RouterModule, MatIconModule, MatButtonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile {
  private userService = inject(UsersService);
  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);

  user = signal<UserDTO | null>(null);
  currentUserId = signal(this.authService.getUserData()?.id);
  posts = signal<PostDTO[]>([]);
  defaultImage = 'imgs/default-user.png';

  constructor()
  {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.route.paramMap.subscribe(params => {
        const id = Number(params.get('id'));
        this.loadUser(id);
      });

    
  }

  loadUser(id: number | null) {
    if (id) {
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

}
