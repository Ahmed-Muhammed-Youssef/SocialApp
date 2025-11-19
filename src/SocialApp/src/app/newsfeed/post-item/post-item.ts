import { Component, Input } from '@angular/core';
import { PostDTO } from '../models/post-dto';
import { MatCardModule } from '@angular/material/card';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-post-item',
  imports: [MatCardModule, DatePipe],
  templateUrl: './post-item.html',
  styleUrl: './post-item.css',
})
export class PostItem {
  @Input() post!: PostDTO;
}
