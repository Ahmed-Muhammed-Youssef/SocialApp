import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/_models/User';
import { GalleryModule, GalleryItem, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css'],
  standalone: true,
  imports: [GalleryModule]
})
export class GalleryComponent implements OnInit{
  @Input() user: User | undefined;
  images: GalleryItem[] = [];
  ngOnInit(): void {
    if(this.user && this.user.pictures.length > 0)
    {
      this.user.pictures.forEach(p => this.images.push(new ImageItem({ src: p.url, thumb: p.url })))
    }
  }

}
