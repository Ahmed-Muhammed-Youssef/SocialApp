import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css']
})
export class GalleryComponent implements OnInit{
  @Input() user: User | undefined;
  ngOnInit(): void {
    
  }

}
