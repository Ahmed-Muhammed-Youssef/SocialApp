import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {DragDropModule} from '@angular/cdk/drag-drop';
import {LayoutModule} from '@angular/cdk/layout';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatSidenavModule} from '@angular/material/sidenav';
import {MatButtonToggleModule} from '@angular/material/button-toggle';

import { NavComponent } from './nav/nav.component';
import { FooterComponent } from './footer/footer.component';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { UserListComponent } from './users/user-list/user-list.component';
import { UserDetailComponent } from './users/user-detail/user-detail.component';
import { ListsComponent } from './lists/lists.component';
import { ChatComponent } from './chat/chat.component';
import { AuthGuard } from './_guards/auth.guard';
import { SharedModule } from './_modules/shared.module';
import { LoginComponent } from './login/login.component';
import { TestErrorComponent } from './errors/test-error/test-error.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { UserCardComponent } from './users/user-card/user-card.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { NgxGalleryModule } from '@kolkov/ngx-gallery';
import { UserEditComponent } from './users/user-edit/user-edit.component';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { NgxSpinnerModule } from "ngx-spinner";
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { PhotoEditorComponent } from './users/photo-editor/photo-editor.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { UserDetailedResolver } from './_resolvers/user-detailed.resolver';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { AdminGuard } from './_guards/admin.guard';
import { HasRoleDirective } from './_directives/has-role.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { PhotoManagementComponent } from './admin/photo-management/photo-management.component';
import { RoleEditDialogComponent } from './admin/role-edit-dialog/role-edit-dialog.component';
import { MobileNavComponent } from './mobile-nav/mobile-nav.component';
import { ContactComponent } from './contact/contact.component';


const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'users', component: UserListComponent, canActivate:[AuthGuard] },
  { path: 'users/username/:username', component: UserDetailComponent, canActivate:[AuthGuard], resolve: {user: UserDetailedResolver} },
  { path: 'lists', component: ListsComponent, canActivate:[AuthGuard] },
  { path: 'messages', component: ContactComponent, canActivate: [AuthGuard] },
  { path: 'chat', component: ChatComponent, canActivate: [AuthGuard] },
  { path: 'admin', component: AdminPanelComponent, canActivate: [AuthGuard, AdminGuard]},
  { path: 'users/edit', component: UserEditComponent, canActivate: [AuthGuard], canDeactivate: [PreventUnsavedChangesGuard] },
  /*{
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'users', component: UserListComponent },
      { path: 'users/username/:username', component: UserDetailComponent },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent }
    ]
  },*/
  { path: 'errors', component: TestErrorComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '', redirectTo:'/home', pathMatch:'full' },
  { path: '**', redirectTo:'/not-found', pathMatch:'full' }
];

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    FooterComponent,
    HomeComponent,
    RegisterComponent,
    UserListComponent,
    UserDetailComponent,
    ListsComponent,
    ChatComponent,
    LoginComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    UserCardComponent,
    UserEditComponent,
    PhotoEditorComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    PhotoManagementComponent,
    RoleEditDialogComponent,
    MobileNavComponent,
    ContactComponent
  ],
  imports: [
    BrowserModule, HttpClientModule,
    BrowserAnimationsModule, FormsModule,
    ReactiveFormsModule, RouterModule.forRoot(routes),
    SharedModule, HttpClientModule,
    NgxGalleryModule, NgxSpinnerModule,
    DragDropModule, FontAwesomeModule, LayoutModule,
    MatSidenavModule, MatButtonToggleModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true}
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
