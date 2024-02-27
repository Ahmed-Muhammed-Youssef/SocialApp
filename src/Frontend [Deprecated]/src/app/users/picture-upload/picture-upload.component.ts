import { HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Picture, User } from 'src/app/_models/User';
import { PictureService } from 'src/app/_services/picture.service';
@Component({
  selector: 'app-picture-upload',
  templateUrl: './picture-upload.component.html',
  styleUrls: ['./picture-upload.component.css']
})
export class PictureUploadComponent implements OnInit {
  @Input() user: User | undefined = undefined;
  @Output() pictureUploaded = new EventEmitter<Picture>();
  imagePreviewUrl: string = "#";
  file: File | null = null;
  imageUploadProgress: number = 0;
  constructor(private imageService: PictureService, private toastr: ToastrService) { }
  ngOnInit(): void {}
  UploadImage(){
    if(this.file)
    {
      this.imageService.uploadImage(this.file).subscribe(event => {
        if (event.type === HttpEventType.UploadProgress){
          this.imageUploadProgress = Math.round(100 * event.loaded / event.total);
        }
        else if(event.type === HttpEventType.Response){
          this.toastr.success('Image uploaded successfully');
          this.imagePreviewUrl = "#";
          this.pictureUploaded.emit(event.body);
        }
      });
    }
  }
  DisplayImage(e : any){
    if (e.target.files && e.target.files[0]) {
        this.file = e.target.files[0];
        var reader = new FileReader();
        reader.readAsDataURL(e.target.files[0]); // read file as data url
        reader.onload = (event) => { // called once readAsDataURL is completed
          if(event.target!.result)
          this.imagePreviewUrl = event.target!.result as string;
      }
    }
  }
  CancelUpload(){
    this.imagePreviewUrl = '#'
    this.file = null;
  }
}