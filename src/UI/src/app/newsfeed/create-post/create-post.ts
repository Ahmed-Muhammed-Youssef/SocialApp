import { inject, output, signal, Signal } from '@angular/core';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewsfeedService } from '../services/newsfeed';
import { CreatePostRequest } from '../models/create-post-request';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatHint } from '@angular/material/form-field';

@Component({
  selector: 'app-create-post',
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatHint
  ],
  templateUrl: './create-post.html',
  styleUrl: './create-post.css',
})
export class CreatePost {
  postContent = signal('');
  isSubmitting = signal(false);
  postCreated = output<number>();
  newsfeedService = inject(NewsfeedService);

  createPost() {
    if (!this.postContent().trim()) {
      return;
    }

    this.isSubmitting.set(true);
    const newPost: CreatePostRequest = { content: this.postContent() };

    this.newsfeedService.createPost(newPost).subscribe({
      next: (postId) => {
        console.log('Post created:', postId);
        this.postCreated.emit(postId);
        this.postContent.set('');
        this.isSubmitting.set(false);
      },
      error: (error) => {
        console.error('Error creating post:', error);
        this.isSubmitting.set(false);
      },
    });
  }
}
