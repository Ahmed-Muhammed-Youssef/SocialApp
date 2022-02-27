import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { LoginResponse } from 'src/app/_models/AccountModels';
import { Photo, User } from 'src/app/_models/User';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';
import { AccountService } from 'src/app/_services/account.service';
@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() user: User | undefined = undefined;  
  account: LoginResponse|null = null; 
  uploader: FileUploader = new FileUploader({});
  hasBaseDropzoneOver = false;
  constructor(private accountService: AccountService) {
    accountService.currentUser$.pipe(take(1)).subscribe(acc => {
      if(acc){
        this.account = acc;
      }
    });
  }
  initializeUploader() {
    this.uploader = new FileUploader({
      url: '/api/users/photo/upload',
      authToken: 'Bearer ' + this.account?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.user?.photos?.push(photo);
      }
    }
  }
  drop(event: any) {
    moveItemInArray(this.user?.photos as Photo[], event.previousIndex, event.currentIndex);
  }
  fileOverBase(event: any){
    this.hasBaseDropzoneOver = event;
  }
  ngOnInit(): void {
    this.initializeUploader();
  }

}
