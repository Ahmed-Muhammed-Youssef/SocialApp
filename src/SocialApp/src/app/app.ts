import { Component, signal } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { GlobalLoader } from './shared/global-loader/global-loader';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterModule, GlobalLoader],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('SocialApp');
}
