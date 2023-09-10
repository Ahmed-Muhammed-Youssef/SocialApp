import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Picture } from 'src/app/_models/User';
import { PictureService } from 'src/app/_services/picture.service';

@Component({
  selector: 'app-gallery',
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css']
})
export class GalleryComponent implements OnInit {
  @Input() pictures: Picture[] = [];
  activeIndex = 0;
  constructor(private pictureSerive: PictureService, private toastr: ToastrService, private changeDetectorRef : ChangeDetectorRef) { }
  ngOnInit(): void {
  }
  prevImage() {
    this.activeIndex = this.activeIndex > 0 ? this.activeIndex - 1 : this.pictures.length - 1;
  }

  nextImage() {
    this.activeIndex = (this.activeIndex + 1) % this.pictures.length;
  }
  deleteImage(pictureIndex: number) {
    this.pictureSerive.deletePicture(this.pictures[pictureIndex].id).subscribe(r => {
      this.toastr.success('Picture deleted successfully!');
      this.pictures.splice(pictureIndex, 1);
      this.changeDetectorRef.detectChanges();
      console.log(this.pictures);
    });
  }
}
