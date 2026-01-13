import { Component } from '@angular/core';
import { PostList } from '../post-list/post-list';

@Component({
  selector: 'app-newsfeed-page',
  imports: [PostList],
  templateUrl: './newsfeed-page.html',
  styleUrl: './newsfeed-page.css',
})
export class NewsfeedPage {

}
