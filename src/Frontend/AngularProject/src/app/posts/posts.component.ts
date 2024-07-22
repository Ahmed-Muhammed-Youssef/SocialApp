import { Component } from '@angular/core';
import { PostCardComponent } from '../post-card/post-card.component';

@Component({
  selector: 'app-posts',
  standalone: true,
  imports: [PostCardComponent],
  templateUrl: './posts.component.html',
  styleUrl: './posts.component.css'
})
export class PostsComponent {
  testImage : string = 'https://material.angular.io/assets/img/examples/shiba2.jpg';
  testProfilePicture: string = 'https://www.vhv.rs/dpng/d/505-5058560_person-placeholder-image-free-hd-png-download.png';
  testPostText: string = 'The Shiba Inu is the smallest of the six original and distinct spitz breeds of dog from Japan. small, agile dog that copes very well with mountainous terrain, the Shiba Inu was originally bred for hunting.';
  testUserName = 'Ahmed.M Youssef'
}
