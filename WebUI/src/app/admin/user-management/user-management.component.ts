import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { RoleUser } from 'src/app/_models/roles';
import { AdminService } from 'src/app/_services/admin.service';
import { RoleEditDialogComponent } from '../role-edit-dialog/role-edit-dialog.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  usersRoles: RoleUser[] = [];
  allRoles: string[] = [];
  constructor(adminService: AdminService, public dialog: MatDialog) { 
    adminService.getUsersWithRoles().subscribe(
      r => {
        if(r){
          this.usersRoles = r;
        }
      }
    );
    adminService.getAllRoles().subscribe(
      r => {
        if(r){
          this.allRoles = r;
        }
      }
    );
  }
  openDialog(roleUser: RoleUser){
    const dialogRef = this.dialog.open(RoleEditDialogComponent, {data: roleUser});

    dialogRef.afterClosed().subscribe(
      result => { 
        if(result){
          roleUser.roles = result;
        }
    });
  }

  ngOnInit(): void {
  }

}

