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
import { UserPicturesService } from '../services/user-pictures';
import { UserPictures } from '../user-pictures/user-pictures';

@Component({
  selector: 'app-profile',
  imports: [MatCardModule, DatePipe, PostItem, RouterModule, MatIconModule, MatButtonModule, UserPictures],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile {
  private userService = inject(UsersService);
  private route = inject(ActivatedRoute);
  private authService = inject(AuthService);
  private userPicturesService = inject(UserPicturesService);

  user = signal<UserDTO | null>(null);
  currentUserId = signal(this.authService.getUserData()?.id);
  posts = signal<PostDTO[]>([]);
  userId = signal<number | null>(null);
  defaultImage = 'imgs/default-user.png';

  constructor()
  {
    this.route.paramMap.subscribe(params => {
        const id = Number(params.get('id'));
        this.userId.set(id);
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

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;

    if (!input.files || input.files.length === 0) {
      return;
    }
    this.userPicturesService.uploadUserPicture(input.files[0]).subscribe({
      next: (res) => {
        console.log(res);
        input.value = '';
      },
      error: (err) => {
        console.error(err);
        input.value = '';
      }
    });   
  }

}
