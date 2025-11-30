import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Login } from './auth/login/login';
import { Signup } from './auth/signup/signup';
import { NewsfeedPage } from './newsfeed/newsfeed-page/newsfeed-page';
import { Profile } from './user/profile/profile';
import { authenticatedGuard } from './auth/guards/authenticated-guard';
import { notAuthenticatedGuard } from './auth/guards/not-authenticated-guard';

export const routes: Routes = [
    {path: '', component: Home, canActivate: [notAuthenticatedGuard]},
    {path: 'login', component: Login, canActivate: [notAuthenticatedGuard]},
    {path: 'signup', component: Signup, canActivate: [notAuthenticatedGuard]},
    {path: 'newsfeed', component: NewsfeedPage, canActivate: [authenticatedGuard]},
    {path: 'profile/:id', component: Profile, canActivate: [authenticatedGuard]}
];
