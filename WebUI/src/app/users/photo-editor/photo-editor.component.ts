import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { LoginResponse } from 'src/app/_models/AccountModels';
import { Pictures, User } from 'src/app/_models/User';
import {moveItemInArray} from '@angular/cdk/drag-drop';
import { AccountService } from 'src/app/_services/account.service';
import { UserService } from 'src/app/_services/user.service';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() user: User | undefined = undefined;  
  baseUrl = environment.apiUrl;
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
    let initialPhotos:Pictures[] = this.account?.userData.pictures as Pictures[]; 
    for (let index = 0; index < initialPhotos.length; index++) {
      if(this.user?.pictures[index].id != initialPhotos[index].id){
        return true;
      }
    }      
    return false;
  }
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/photo/upload',
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
        const photo: Pictures = JSON.parse(response);
        this.user?.pictures?.push(photo);
        if(this.account){
          this.user?.pictures.forEach(p =>  this.account?.userData.pictures.push(Object.assign({}, p)));
          this.accountService.setCurrentUser(this.account);
        }
      }
    }
  }
  // reorderPhotos(){
  //   let counter = 0;
  //   this.user?.pictures.forEach(p => {
  //     p.order = counter;
  //     counter++;
  //   });
  //   if(this.user){
  //     this.userService.reorderPhotos(this.user.pictures).subscribe(
  //       response => {
  //         if(response && this.user && this.account){
  //           this.account.userData.pictures = [];
  //           // copying to make sure that the account object 
  //           // is isolated from any other incoming unsaved changes
  //           this.user.pictures.forEach(p =>  this.account?.userData.pictures.push(Object.assign({}, p)));
  //           this.accountService.setCurrentUser(this.account);
  //           this.isOrderChanged = false;
  //         }
  //       }
  //     );
  //   }
  // }
  // deletePhoto(photoId:number){
  //   this.userService.deletePhoto(photoId).subscribe(
  //     () => {
  //       if(this.account && this.user){
  //         const index = this.user.pictures.findIndex(p => p.id === photoId);
  //         this.user.pictures = this.user.pictures.filter(p => p.id !== photoId);
  //         // reassign the order values of the pictures after the deleted picture.
  //         for(let i = index + 1; i < this.user.pictures.length; i++){
  //           this.user.pictures[i].order--;
  //         }
  //         this.account.userData.pictures = [];
  //         // copying to make sure that the account object 
  //         // is isolated from any other incoming unsaved changes
  //         this.user.pictures.forEach(p =>  this.account?.userData.pictures.push(Object.assign({}, p)));
  //         this.accountService.setCurrentUser(this.account);
  //       }
  //     });
  // }
  // drop(event: any) {
  //   moveItemInArray(this.user?.pictures as Pictures[], event.previousIndex, event.currentIndex);
  //   this.isOrderChanged = this.isOrderChangedFunc();
  // }
  // fileOverBase(event: any){
  //   this.hasBaseDropzoneOver = event;
  // }
  ngOnInit(): void {
    this.initializeUploader();
  }

}
