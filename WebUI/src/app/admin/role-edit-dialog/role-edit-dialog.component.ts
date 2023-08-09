import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { MatLegacyDialogRef as MatDialogRef, MAT_LEGACY_DIALOG_DATA as MAT_DIALOG_DATA } from '@angular/material/legacy-dialog';
import { RoleUser } from 'src/app/_models/roles';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-role-edit-dialog',
  templateUrl: './role-edit-dialog.component.html',
  styleUrls: ['./role-edit-dialog.component.css']
})
export class RoleEditDialogComponent implements OnInit {
  roles: string[] = [];
  rolesToAdd: string[] = [];
  rolesToDelete: string[] = [];
  rolesForm: {[role:string]: boolean} = {};
  constructor( public dialogRef: MatDialogRef<RoleEditDialogComponent>,
     @Inject(MAT_DIALOG_DATA) public data: RoleUser, private adminSerive:AdminService) { 
      adminSerive.getAllRoles().subscribe(r => this.roles = r);
      for(let role of this.roles){
        this.rolesForm[role] =  this.data.roles.includes(role);
      }
  }

  ngOnInit(): void {
  }
  onNoClick(): void {
    this.dialogRef.close();
  }
  isRolesChanged(){
    let role : any;
    for(role in this.rolesForm){
      if(this.rolesForm[role] === false){
        if(this.data.roles.includes(role)){
          return true;
        }
      }
      else if(this.rolesForm[role] === true){
        if(!this.data.roles.includes(role)){
          return true;
        }
      }
    }
    return false;
  }
  private whichRoleChanged(){
    let role : any;
    for(role in this.rolesForm){

      if(this.rolesForm[role] === false){
        if(this.data.roles.includes(role)){
          this.rolesToDelete.push(role);
        }
      }
      else if(this.rolesForm[role] === true){
        if(!this.data.roles.includes(role)){
          this.rolesToAdd.push(role);
        }
      }
    }
    return false;
  }
  updateRoles(){
    this.whichRoleChanged();
    for(let role of this.rolesToAdd){
      this.adminSerive.addRoleToUser(this.data.username, role).subscribe(r => {
        this.data.roles.push(role);
      });
    }
    for(let role of this.rolesToDelete){
      this.adminSerive.deleteRoleFromUser(this.data.username, role).subscribe((r) => {
        this.data.roles = this.data.roles.filter(r => r != role);
      });
    }
    
  }
}
