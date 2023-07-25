import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {MatLegacyTabsModule as MatTabsModule} from '@angular/material/legacy-tabs';
import { ToastrModule } from 'ngx-toastr';
import {MatLegacyButtonModule as MatButtonModule} from '@angular/material/legacy-button';
import {MatLegacyFormFieldModule as MatFormFieldModule} from '@angular/material/legacy-form-field';
import {MatIconModule} from '@angular/material/icon';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatLegacyInputModule as MatInputModule} from '@angular/material/legacy-input';
import { MatLegacyCardModule as MatCardModule } from '@angular/material/legacy-card';
import {MatLegacySelectModule as MatSelectModule} from '@angular/material/legacy-select';
import {MatDatepickerModule} from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import {MatLegacyProgressBarModule as MatProgressBarModule} from '@angular/material/legacy-progress-bar';
import {MatLegacyTableModule as MatTableModule} from '@angular/material/legacy-table';
import {MatLegacyMenuModule as MatMenuModule} from '@angular/material/legacy-menu';
import {MatLegacyDialogModule as MatDialogModule} from '@angular/material/legacy-dialog';
import {MatLegacyCheckboxModule as MatCheckboxModule} from '@angular/material/legacy-checkbox';
import {MatLegacyPaginatorModule as MatPaginatorModule} from '@angular/material/legacy-paginator';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatLegacyProgressSpinnerModule as MatProgressSpinnerModule} from '@angular/material/legacy-progress-spinner';

@NgModule({
  declarations: [],
  imports: [
    CommonModule, ToastrModule.forRoot({ positionClass: 'toast-bottom-right' }),
    MatTabsModule, MatButtonModule, 
    MatFormFieldModule, MatIconModule, MatGridListModule, MatInputModule,
    MatCardModule, MatSelectModule, MatDatepickerModule, MatNativeDateModule,
    MatProgressBarModule, MatTableModule, MatMenuModule, MatDialogModule,
    MatCheckboxModule, MatPaginatorModule, MatToolbarModule, MatSidenavModule,
    MatProgressSpinnerModule
  ],
  exports: [
    
    ToastrModule,   
    MatTabsModule, 
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatGridListModule,
    MatInputModule,
    MatCardModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressBarModule,
    MatTableModule,
    MatMenuModule,
    MatDialogModule,
    MatCheckboxModule,
    MatPaginatorModule,
    MatToolbarModule,
    MatSidenavModule,
    MatProgressSpinnerModule
    ]
})
export class SharedModule { }
