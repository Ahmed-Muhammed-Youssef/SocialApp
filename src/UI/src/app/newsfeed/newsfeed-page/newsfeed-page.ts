import { Component, ViewChild } from '@angular/core';
import { PostList } from '../post-list/post-list';
import { CreatePost } from '../create-post/create-post';
import { PostDTO } from '../models/post-dto';

@Component({
  selector: 'app-newsfeed-page',
  imports: [PostList, CreatePost],
  templateUrl: './newsfeed-page.html',
  styleUrl: './newsfeed-page.css',
})
export class NewsfeedPage {
  @ViewChild(PostList) postList!: PostList;

  onPostCreated(postId: number) {
    // Refresh the post list to display the newly created post
    console.log('New post created:', postId);
    // this.postList?.refreshPosts();
  }
}
