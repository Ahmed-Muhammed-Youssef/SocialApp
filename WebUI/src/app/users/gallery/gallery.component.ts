import { ChangeDetectorRef, Component, Input, OnInit, Renderer2 } from '@angular/core';
import { Picture } from 'src/app/_models/User';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css']
})
export class GalleryComponent implements OnInit{
  @Input() pictures: Picture[] = [];
  activeIndex = 0;
  constructor(private changeDetectorRef: ChangeDetectorRef){}
  ngOnInit(): void {
  }
  prevImage() {
    this.activeIndex = this.activeIndex > 0 ? this.activeIndex - 1 : this.pictures.length - 1;
  }
  
  nextImage() {
    this.activeIndex = (this.activeIndex + 1) % this.pictures.length;
    
  }
}
