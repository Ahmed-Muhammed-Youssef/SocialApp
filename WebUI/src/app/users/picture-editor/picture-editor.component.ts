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
  constructor(private imageService: PictureService) { }
  ngOnInit(): void {}
}