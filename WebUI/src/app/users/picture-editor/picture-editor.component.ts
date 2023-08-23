import { Component, Input, OnInit } from '@angular/core';
import { User } from 'src/app/_models/User';
import { PictureService } from 'src/app/_services/picture.service';
@Component({
  selector: 'app-picture-editor',
  templateUrl: './picture-editor.component.html',
  styleUrls: ['./picture-editor.component.css']
})
export class PictureEditorComponent implements OnInit {
  @Input() user: User | undefined = undefined;
  imageInput = document.getElementById('imageInput');
  imagePreviewUrl: string = "#";
  constructor(private imageService: PictureService) { }
  ngOnInit(): void {}
  UpdatePreview(event : any)
  {
    if (event.target.files && event.target.files[0]) {
      var reader = new FileReader();

      reader.readAsDataURL(event.target.files[0]); // read file as data url

      reader.onload = (event) => { // called once readAsDataURL is completed
        if(event.target!.result)
        this.imagePreviewUrl = event.target!.result as string;
      }
    }
  }
}