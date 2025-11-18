import { Routes } from '@angular/router';
import { Home } from './home/home';
import { Login } from './auth/login/login';
import { Signup } from './auth/signup/signup';
import { NewsfeedPage } from './newsfeed/newsfeed-page/newsfeed-page';

export const routes: Routes = [
    {path: '', component: Home},
    {path: 'login', component: Login},
    {path: 'signup', component: Signup},
    {path: 'newsfeed', component: NewsfeedPage}
];
