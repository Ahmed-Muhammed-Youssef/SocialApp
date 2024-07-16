import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MainBarComponent} from './main-bar/main-bar.component'

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MainBarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'AngularProject';
}
