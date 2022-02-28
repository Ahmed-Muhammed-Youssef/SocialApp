import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { LoginResponse } from 'src/app/_models/AccountModels';
import { Photo, User } from 'src/app/_models/User';
import {moveItemInArray} from '@angular/cdk/drag-drop';
import { AccountService } from 'src/app/_services/account.service';
import { UserService } from 'src/app/_services/user.service';
@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() user: User | undefined = undefined;  
  account: LoginResponse|null = null; 
  isOrderChanged = false;
  uploader: FileUploader = new FileUploader({});
  hasBaseDropzoneOver = false;
  constructor(private accountService: AccountService, private userService:UserService) {
    accountService.currentUser$.pipe(take(1)).subscribe(acc => {
      if(acc){
        this.account = acc;
      }
    });
  }
  isOrderChangedFunc(): boolean{
    let initialPhotos:Photo[] = this.account?.userData.photos as Photo[]; 
    for (let index = 0; index < initialPhotos.length; index++) {
      if(this.user?.photos[index].id != initialPhotos[index].id){
        return true;
      }
    }      
    return false;
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
  reorderPhotos(){
    let counter = 0;
    this.user?.photos.forEach(p => {
      p.order = counter;
      counter++;
    });
    if(this.user){
      this.userService.reorderPhotos(this.user.photos).subscribe(
        response => {
          if(response && this.user && this.account){
            this.account.userData.photos = [];
            // copying to make sure that the account object 
            // is isolated from any other incoming unsaved changes
            this.user.photos.forEach(p =>  this.account?.userData.photos.push(Object.assign({}, p)));
            this.accountService.setCurrentUser(this.account)
            this.isOrderChanged = false;
          }
        }
      );
    }
    // this.initialPhotos = [];
    // this.user?.photos.forEach(p => {
    //   this.initialPhotos.push( {
    //     id: p.id,
    //     url: p.url,
    //     order: p.order
    //   });
    // });
  }
  drop(event: any) {
    moveItemInArray(this.user?.photos as Photo[], event.previousIndex, event.currentIndex);
    this.isOrderChanged = this.isOrderChangedFunc();
  }
  fileOverBase(event: any){
    this.hasBaseDropzoneOver = event;
  }
  ngOnInit(): void {
    this.initializeUploader();
  }

}
